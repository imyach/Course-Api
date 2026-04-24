namespace CourseWebApi.Servises
{
    public class GenerateRandomValueService : IGenerateRandomValueService
    {
        private static readonly Random _random = new Random();

        public string GenerateNewPassword()
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%^&*()_-+=<>?";

            const string allChars = upperCase + lowerCase + digits + special;

            var random = new Random();
            var password = new char[8];

            password[0] = upperCase[random.Next(upperCase.Length)];
            password[1] = lowerCase[random.Next(lowerCase.Length)];
            password[2] = digits[random.Next(digits.Length)];
            password[3] = special[random.Next(special.Length)];

            for (int i = 4; i < 8; i++)
            {
                password[i] = allChars[random.Next(allChars.Length)];
            }

            return new string(password.OrderBy(x => random.Next()).ToArray());
        }

        public string GenerateRecoveryCode()
        {
            return _random.Next(0, 999999).ToString("D6");
        }
    }
}
