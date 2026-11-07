using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace KhedutRise
{
    // Result returned by SMS Gateway delivery attempts
    public class SmsResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string MessageId { get; set; }
        public string RawResponse { get; set; }

        public static SmsResult SuccessResult(string messageId, string message = "SMS delivered successfully.", string rawResponse = "")
        {
            return new SmsResult
            {
                Success = true,
                Message = message,
                MessageId = messageId,
                RawResponse = rawResponse
            };
        }

        public static SmsResult FailureResult(string message, string rawResponse = "")
        {
            return new SmsResult
            {
                Success = false,
                Message = message,
                RawResponse = rawResponse
            };
        }
    }

    // Reusable SMS Gateway Service supporting Fast2SMS, MSG91, and Development Simulation
    public static class SmsService
    {
        // Send a 6-digit verification OTP to the target Indian mobile number
        public static SmsResult SendOtp(string mobileNumber, string otp)
        {
            // 1. Sanitize and validate 10-digit mobile number
            string cleanMobile = CleanMobile(mobileNumber);
            if (string.IsNullOrEmpty(cleanMobile) || !Regex.IsMatch(cleanMobile, @"^[6-9]\d{9}$"))
            {
                return SmsResult.FailureResult("Invalid mobile number. Please provide a valid 10-digit Indian mobile number.");
            }

            if (string.IsNullOrWhiteSpace(otp) || otp.Length != 6)
            {
                return SmsResult.FailureResult("Invalid OTP. Verification code must be exactly 6 digits.");
            }

            // 2. Format the standardized professional SMS template
            string smsBody = string.Format(
                "Your KhedutRise verification OTP is {0}. It is valid for 5 minutes. Do not share this code with anyone.",
                otp);

            // 3. Retrieve Gateway configurations
            string provider = ConfigurationManager.AppSettings["SmsProvider"] ?? "Fast2SMS";
            string apiKey = Environment.GetEnvironmentVariable("KHEDUTRISE_SMS_API_KEY") 
                            ?? ConfigurationManager.AppSettings["SmsApiKey"] 
                            ?? string.Empty;
            apiKey = apiKey.Trim();

            bool isPlaceholderKey = string.IsNullOrEmpty(apiKey) 
                                    || apiKey.Contains("YOUR_SMS") 
                                    || apiKey.Contains("YOUR_FAST2SMS");

            bool simulateInDev = (ConfigurationManager.AppSettings["SmsSimulateInDev"] ?? "true")
                                 .Equals("true", StringComparison.OrdinalIgnoreCase);

            // 4. Handle development simulation if no valid key is present
            if (isPlaceholderKey)
            {
                if (simulateInDev)
                {
                    string simId = "DEV-SIM-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
                    Trace.WriteLine(string.Format("[SMS SIMULATED] To: +91 {0} | Body: {1}", cleanMobile, smsBody));
                    return SmsResult.SuccessResult(simId, "OTP dispatched via SMS (Development Mode).", "Simulated delivery");
                }
                else
                {
                    return SmsResult.FailureResult("SMS Gateway API key is not configured. Please set SmsApiKey in Web.config.");
                }
            }

            // 5. Dispatch via configured Gateway provider
            try
            {
                // Ensure TLS 1.2+ for gateway calls
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                if (provider.Equals("Fast2SMS", StringComparison.OrdinalIgnoreCase))
                {
                    return SendViaFast2Sms(cleanMobile, otp, smsBody, apiKey);
                }
                else if (provider.Equals("MSG91", StringComparison.OrdinalIgnoreCase))
                {
                    return SendViaMsg91(cleanMobile, otp, apiKey);
                }
                else
                {
                    return SmsResult.FailureResult("Unsupported SMS provider specified in configuration: " + provider);
                }
            }
            catch (WebException webEx)
            {
                string responseText = "";
                if (webEx.Response != null)
                {
                    using (StreamReader reader = new StreamReader(webEx.Response.GetResponseStream()))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
                Trace.WriteLine("[SMS Gateway Error] " + webEx.Message + " | Response: " + responseText);
                return SmsResult.FailureResult("SMS gateway connection error. Please verify network or try again.", responseText);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("[SMS Error] " + ex.Message);
                return SmsResult.FailureResult("SMS delivery failed: " + ex.Message);
            }
        }

        // Fast2SMS Gateway integration (POST https://www.fast2sms.com/dev/bulkV2)
        private static SmsResult SendViaFast2Sms(string mobile, string otp, string message, string apiKey)
        {
            string url = "https://www.fast2sms.com/dev/bulkV2";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("authorization", apiKey);
            request.Timeout = 12000; // 12 seconds timeout

            // Use the Quick/OTP route for Fast2SMS
            string jsonPayload = string.Format(
                "{{\"route\":\"otp\",\"variables_values\":\"{0}\",\"numbers\":\"{1}\"}}",
                otp, mobile);

            byte[] byteData = Encoding.UTF8.GetBytes(jsonPayload);
            request.ContentLength = byteData.Length;

            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string jsonResponse = reader.ReadToEnd();
                // Fast2SMS returns: {"return":true,"request_id":"...","message":["OTP sent successfully"]}
                if (jsonResponse.Contains("\"return\":true") || jsonResponse.Contains("\"return\": true"))
                {
                    string reqId = ExtractJsonValue(jsonResponse, "request_id");
                    return SmsResult.SuccessResult(reqId, "SMS OTP sent successfully via Fast2SMS.", jsonResponse);
                }
                else
                {
                    string err = ExtractJsonValue(jsonResponse, "message");
                    return SmsResult.FailureResult("Fast2SMS delivery rejected: " + (string.IsNullOrEmpty(err) ? jsonResponse : err), jsonResponse);
                }
            }
        }

        // MSG91 Gateway integration
        private static SmsResult SendViaMsg91(string mobile, string otp, string authKey)
        {
            string templateId = ConfigurationManager.AppSettings["Msg91TemplateId"] ?? "";
            string url = string.Format("https://control.msg91.com/api/v5/otp?template_id={0}&mobile=91{1}&otp={2}",
                Uri.EscapeDataString(templateId), Uri.EscapeDataString(mobile), Uri.EscapeDataString(otp));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Headers.Add("authkey", authKey);
            request.ContentType = "application/json";
            request.ContentLength = 0;
            request.Timeout = 12000;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string jsonResponse = reader.ReadToEnd();
                if (jsonResponse.Contains("\"type\":\"success\"") || jsonResponse.Contains("\"type\": \"success\""))
                {
                    string msgId = ExtractJsonValue(jsonResponse, "message");
                    return SmsResult.SuccessResult(msgId, "SMS OTP sent successfully via MSG91.", jsonResponse);
                }
                else
                {
                    return SmsResult.FailureResult("MSG91 delivery error: " + jsonResponse, jsonResponse);
                }
            }
        }

        // Clean mobile number string into 10-digit number
        public static string CleanMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile)) return string.Empty;
            string clean = Regex.Replace(mobile, @"[^\d]", "");
            if (clean.StartsWith("91") && clean.Length == 12)
            {
                clean = clean.Substring(2);
            }
            else if (clean.StartsWith("0") && clean.Length == 11)
            {
                clean = clean.Substring(1);
            }
            return clean;
        }

        // Helper to extract string value from simple JSON
        private static string ExtractJsonValue(string json, string key)
        {
            try
            {
                Match match = Regex.Match(json, "\"" + Regex.Escape(key) + "\"\\s*:\\s*\"?([^,\"\\]}]+)\"?");
                return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
