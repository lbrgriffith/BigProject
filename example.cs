try
            {
                int zero = 0;
                Console.WriteLine($"{MethodBase.GetCurrentMethod().Name}");
                int result = 5 / zero;
            }
            catch (DivideByZeroException ex)
            {
                // add custom message and pass in the exception
                Log.Error(ex, MethodBase.GetCurrentMethod().Name, "Whoops!");
                Console.ReadKey();
            }
