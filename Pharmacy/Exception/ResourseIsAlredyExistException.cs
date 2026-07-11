namespace Pharmasy.Exception;

public class ResourseIsAlredyExistException : System.Exception
{
    public ResourseIsAlredyExistException(string massage) : base(massage)
    {
    }
}