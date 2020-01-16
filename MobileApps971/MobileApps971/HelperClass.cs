using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApps971
{
    public class HelperClass
    {
        public static bool IsNull(string emptyField)
        {
            //checks for null values
            if (String.IsNullOrEmpty(emptyField))
                return true;
            return false;
        }

        public static bool EmailValidation(string emptyField)
        {
            //write code here
            return false;
        }

        public static bool EmailIsValid(string email)
        {
            //Checks to make sure email is entered with no spaces
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            //Checks for WGU Email Account
            if (!email.ToLower().Contains("@wgu.edu"))
            {
                return false;
            }

            return true;

        }

        public static bool PhoneIsValid(string phone)
        {
            if (phone.Length != 10)
            {
                return false;
            }

            try
            {
                var IntPhone = Convert.ToInt64(phone);
            }
            catch (FormatException)
            {

                return false;
            } 


            return true;
        }
    }
}
