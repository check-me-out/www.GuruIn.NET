namespace WebFramework.Helpers
{
    using System.IO;
    using System.Reflection;

    public static class EmbeddedResource
    {
        public static string GetContents(string resourceName)
        {
            var result = string.Empty;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }

            return result;
        }
    }
}