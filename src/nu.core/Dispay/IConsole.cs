namespace nu.Utility
{
   public interface IConsole
   {
      void WriteHeading(string message);
      void WriteError(string message);
      void WriteLine(string message);
      void WriteLine(string message, params object[] args);
      void WriteBlankLine();
      void WriteSeparator();
   }
}