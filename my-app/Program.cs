using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneratorApp
{
  public class App
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Do you want to see the whole process? (Y/N):");
      string showcase = Console.ReadLine();
      bool continuous_save = false;

      if (showcase.ToUpper() == "Y")
      {
        Console.WriteLine("Do you want to save the whole process? (Y/N):");
        string continuous_save_text = Console.ReadLine();
        if (continuous_save_text.ToUpper() == "Y") { continuous_save = true; }
      }

      BiomeGenerator bg = new BiomeGenerator();
      bg.continuous_save = continuous_save;
      Form form = new Form();
      form.Width = 400;
      form.Height = 400;

      if (showcase.ToUpper() == "Y") 
      {
        bg.showcase = true;
        bg.form = form;

        Task.Run(() => {
          Application.Run(bg.form);
          bg.form.BringToFront();
          bg.form.Activate();
        });
        
        bg.form.Text = "Generation Showcase";
      }

      while (!bg.finished) 
      {
        try { bg.Update(); } 
        catch(Exception e) { Console.WriteLine(e.ToString()); }
        if (bg.finished) 
        {
          Console.WriteLine("FINISHED");
          Console.WriteLine("Do you want to repeat it? (Y/N):");
          string repeat = Console.ReadLine();
          if (repeat.ToUpper() == "Y")
          {
            bg = new BiomeGenerator();
            bg.continuous_save = continuous_save;
            if (showcase.ToUpper() == "Y")  
            { 
              bg.showcase = true; 
              bg.form = form;
            }
        } }
      }
} } }
