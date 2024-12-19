using FinalTaskOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalTaskOne.Controllers
{
    public class TokenController : Controller
    {
        private TokenModel _tokenModel;

        public TokenController()
        {
            // Initialize _tokenModel to null (it will be initialized properly in the Index action)
            _tokenModel = null;
        }

        // Initialize session and TokenModel only if it's not initialized
        private void InitializeTokenModel()
        {
            if (Session["TokenModel"] == null)
            {
                _tokenModel = new TokenModel();
                Session["TokenModel"] = _tokenModel;
            }
            else
            {
                _tokenModel = (TokenModel)Session["TokenModel"];
            }
        }

        // Display the current token details and state
        public ActionResult Index()
        {
            // Ensure the TokenModel is properly initialized
            InitializeTokenModel();

            return View(_tokenModel);
        }

        // Generate a token for a given counter
        public ActionResult GenerateToken(string counterType)
        {
            // Ensure the TokenModel is properly initialized
            InitializeTokenModel();

            // Check if we have reached the max requests for the day
            if (_tokenModel.RequestsMadeToday >= _tokenModel.MaxRequestsPerDay)
            {
                TempData["Message"] = "Maximum requests for today reached.";
                return RedirectToAction("Index");
            }

            string token = string.Empty;

            // Handle token generation for each counter type
            switch (counterType.ToLower())
            {
                case "medical":
                    if (_tokenModel.MedicalTokensIssued >= _tokenModel.MaxTokensPerCounter)
                    {
                        TempData["Message"] = "Medical counter has reached its max token limit (25).";
                        return RedirectToAction("Index");
                    }
                    token = "Med-" + _tokenModel.GlobalTokenNumber++;
                    _tokenModel.MedicalTokensIssued++;
                    break;
                case "tourist":
                    if (_tokenModel.TouristTokensIssued >= _tokenModel.MaxTokensPerCounter)
                    {
                        TempData["Message"] = "Tourist counter has reached its max token limit (25).";
                        return RedirectToAction("Index");
                    }
                    token = "TR-" + _tokenModel.GlobalTokenNumber++;
                    _tokenModel.TouristTokensIssued++;
                    break;
                case "business":
                    if (_tokenModel.BusinessTokensIssued >= _tokenModel.MaxTokensPerCounter)
                    {
                        TempData["Message"] = "Business counter has reached its max token limit (25).";
                        return RedirectToAction("Index");
                    }
                    token = "BS-" + _tokenModel.GlobalTokenNumber++;
                    _tokenModel.BusinessTokensIssued++;
                    break;
                case "go":
                    if (_tokenModel.GovtOfficialTokensIssued >= _tokenModel.MaxTokensPerCounter)
                    {
                        TempData["Message"] = "Government Officials counter has reached its max token limit (25).";
                        return RedirectToAction("Index");
                    }
                    token = "GO-" + _tokenModel.GlobalTokenNumber++;
                    _tokenModel.GovtOfficialTokensIssued++;
                    break;
            }

            // Increment the daily request count
            _tokenModel.RequestsMadeToday++;

            // Update session with the new state
            Session["TokenModel"] = _tokenModel;

            // Show the generated token
            TempData["Token"] = token;

            return RedirectToAction("Index");
        }

        // Call the next customer (this can be customized based on further logic)
        public ActionResult CallNextCustomer()
        {
            // In this simple implementation, calling next customer doesn't change much.
            // But this could be extended to show which counter is calling the next customer.
            return RedirectToAction("Index");
        }
    }
}