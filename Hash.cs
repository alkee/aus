
namespace aus
{
    public static class Hash
    {
        public static string Md5(this string soruce)
        { // http://wiki.unity3d.com/index.php?title=MD5
            var bytes = System.Text.Encoding.UTF8.GetBytes(soruce);
            var hashed = md5.ComputeHash(bytes);

            var hash = "";
            for (var i = 0; i < hashed.Length; ++i)
                hash += System.Convert.ToString(hashed[i], 16).PadLeft(2, '0');
            return hash.PadLeft(32, '0');
        }
        private static System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

        public static string Sha1(this string soruce)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(soruce);
            var hashed = sha1.ComputeHash(bytes);

            var hash = "";
            for (var i = 0; i < hashed.Length; ++i)
                hash += System.Convert.ToString(hashed[i], 16).PadLeft(2, '0');
            return hash.PadLeft(32, '0');
        }
        private static System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
    }
}
