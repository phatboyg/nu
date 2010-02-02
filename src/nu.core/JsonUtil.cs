namespace nu.core
{
    using System.Web.Script.Serialization;

    public static class JsonUtil
    {
        public static string ToJson(object objectToSerialize)
        {
            return new JavaScriptSerializer().Serialize(objectToSerialize);
        }

        public static T Get<T>(string rawJson)
        {
            return new JavaScriptSerializer().Deserialize<T>(rawJson);
        }

        public static object Get(string rawJson)
        {
            return new JavaScriptSerializer().DeserializeObject(rawJson);
        }
    }
}