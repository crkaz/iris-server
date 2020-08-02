namespace iris_server.Models
{
    public class VisionTags
    {
        public struct TagData
        {
            public string name;
            public float confidence;
        }

        public struct DescriptionData
        {
            public string[] tags;
            public CaptionsData[] captions;
        }

        public struct CaptionsData
        {
            public string text;
            public float confidence;
        }

        public DescriptionData description;
        public TagData[] tags;
    }
}
