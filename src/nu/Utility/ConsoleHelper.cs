namespace nu.Utility
{
   using System;

   public class ConsoleHelper : IConsoleHelper
   {
      public void WriteError(string message)
      {
         string line = string.Format("Error: {0}.", message);
         
         Console.WriteLine();
         Console.WriteLine(line);
         Console.WriteLine();
      }
   }
}