using System.Globalization;

namespace nu.Utility
{
   using System;
   using System.Text;

   public class ConsoleHelper : 
       IConsole
   {
      private string _separator;

      public void WriteError(string message)
      {
         string line = string.Format(CultureInfo.CurrentUICulture, "Error: {0}.", message);
         
         WriteBlankLine();
         WriteLine(line);
      }

      public void WriteLine(string message)
      {
         Console.WriteLine(string.Format(CultureInfo.CurrentUICulture, message));
      }

      public void WriteLine(string message, params object[] args)
      {
         WriteLine(string.Format(CultureInfo.CurrentUICulture, message, args));
      }

      public void WriteHeading(string message)
      {
         WriteBlankLine();
         WriteLine(message);
         WriteSeparator();
      }

      public void WriteBlankLine()
      {
         WriteLine(string.Empty);
      }

      public void WriteSeparator()
      {
         WriteLine(string.Empty);
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