public class PatientFile
{
    public string SerialNumber { get; set; }
    public string Name { get; set; }

    public string UniquePatientID { get; set; }
    public string PhoneNumber { get; set; }
    public string FileStatus { get; set; }
    public string FileName { get; set; }
    public string NameOfFile { get; set; }
    public string FileType { get; set; }
    public string FileNumber { get; set; } // Make sure this property exists
    public string ImagePath { get; set; } // Assuming this is also part of your class
}
