﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.TwiML;

namespace CallCenter.Web.Controllers
{
    public class PhoneController : Controller
    {
        //
        // GET: /Phone/

        public ActionResult IncomingCall(string CallSid)
        {
            var call = GetClient(CallSid);

            TwilioResponse response = new TwilioResponse();
            response.Say("Welcome to the Bank of Griff.");
            response.Pause();
            response.Say("Press 1 to manage your account.");
            response.Say("Press 2 to take out a loan.");
            response.Say("Press 3 to talk to a representative.");
            response.Gather(new {action = Url.Action("ServiceRequest"), timeout = 10, method = "POST", finishOnKey = "", numDigits = 1});

            Stream result = new MemoryStream(Encoding.Default.GetBytes(response.ToString()));

            return new FileStreamResult(result, "text/plain");
        }

        private static Call GetClient(string CallSid)
        {
            string accountSid = "";
            string authToken = "";

            TwilioRestClient client = new TwilioRestClient(accountSid, authToken);
            var call = client.GetCall(CallSid);
            return call;
        }

        public ActionResult ServiceRequest(string CallSid, string Digits)
        {
            var call = GetClient(CallSid);
            TwilioResponse response = new TwilioResponse();
            response.Say(string.Format("You pressed {0}", Digits));
            response.Pause();
            response.Say("Way to go.");
            response.Hangup();

            Stream result = new MemoryStream(Encoding.Default.GetBytes(response.ToString()));

            return new FileStreamResult(result, "text/plain");
        }
    }
}
