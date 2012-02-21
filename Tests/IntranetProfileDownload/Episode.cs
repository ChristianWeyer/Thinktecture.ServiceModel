using System.Drawing;
using System.Runtime.Serialization;

namespace MediaContracts
{
    [DataContract]
    public class Episode
    {
        [DataMember(Name = "ID")]
        public int ID;
        [DataMember(Name = "Title")]
        public string Title;
        [DataMember(Name = "Description")]
        public string Description;
        [DataMember(Name = "Expert")]
        public string Expert;
        [DataMember(Name = "Screenshot")]
        public Bitmap Screenshot;
    }
}
