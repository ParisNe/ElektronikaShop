using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElektronikaShop.Services
{
    public class CaptchaService
    {
        private readonly Random _random =
            new Random();

        public string CurrentCode { get; private set; }

        public void Generate()
        {
            const string symbols =
                "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

            CurrentCode = new string(
                Enumerable.Range(0, 5)
                .Select(x =>
                    symbols[_random.Next(symbols.Length)])
                .ToArray());
        }

        public bool Check(string enteredCode)
        {
            if (string.IsNullOrWhiteSpace(enteredCode))
            {
                return false;
            }

            return string.Equals(
                CurrentCode,
                enteredCode.Trim(),
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
