namespace nu.core.Files
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The 'nu' registry file. Which needs to be kept in sync with the remote registry
    /// </summary>
    [Serializable]
    public class NuRegistry
    {
        public DateTime LastUpdated { get; set; }
        public IList<NugSpec> Nugs { get; set; }

        public static string FileName
        {
            get { return "nu.registry"; }
        }

        public NugSpec GetNugSpec(string name)
        {
            NugSpec result = null;
            foreach (NugSpec nug in Nugs)
            {
                if(nug.Name.Equals(name))
                {
                    result = nug;
                    break;
                }
            }
            return result;
        }
    }
}