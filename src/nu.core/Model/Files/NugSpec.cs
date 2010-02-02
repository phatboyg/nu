namespace nu.core
{
    using System;

    [Serializable]
    public class NugSpec
    {
        public string Name { get; set; }
        public Uri Location { get; set; }
        public string Version { get; set; }
    }
}