namespace FreshCopy.Tests.SampleClasses
{
    public class SampleRecord
    {
        public SampleRecord() { }
        public SampleRecord(string text1)
        {
            Text1 = text1;
        }


        public ulong   Id     { get; set; }
        public string  Text1  { get; set; }
    }
}
