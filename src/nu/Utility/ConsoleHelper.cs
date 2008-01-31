namespace nu.Utility
{
   using System;
   using System.Text;

   public class ConsoleHelper : IConsoleHelper
   {
      private string _separator;

      public void WriteError(string message)
      {
         string line = string.Format("Error: {0}.", message);
         
         Console.WriteLine();
         Console.WriteLine(line);
      }

      public void WriteLine(string message)
      {
         Console.WriteLine(message);
      }

      public void WriteHeading(string message)
      {
         WriteLine(Separator);
         WriteLine(message);
         WriteLine(Separator);
      }

      public string Separator
      {
         get
         {
            return _separator ?? MakeSeparator();
         }
      }

      private string MakeSeparator()
      {
         StringBuilder builder = new StringBuilder();
         for (int c = 1; c <= 80; c++)
         {
            builder.Append("-");
         }
         _separator = builder.ToString();
         return _separator;
      }
   }
}