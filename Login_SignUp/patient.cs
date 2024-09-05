using System.Windows.Media;

public class Patient
{
    public int SerialNumber { get; set; }
    public string UniquePatientID { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string FileNumber { get; set; }
    public string FileName { get; set; }   // New field
    public string FileType { get; set; }   // New field
    public string ScanFilePath { get; set; }
    public ImageSource ImageSource { get; set; }
}
