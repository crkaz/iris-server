using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iris_server.Services
{
    public class DbAccessService
    {
        /// <summary>
        /// Wraps database modification code in try-catch statements to monitor concurrent changes or other mishaps.
        /// </summary>
        /// <param name="action">Database modification to monitor with DbUpdateConcurrencyException</param>
        /// <returns></returns>
        public static bool Attempt(Action action)
        {
            const int RETRIES = 3;

            try
            {
                for (int i = 0; i < RETRIES; i++)
                {
                    try
                    {
                        action.Invoke();
                        return true;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        Console.WriteLine("Failed to fulfil action: database was modified by somebody else");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
            }
            return false;
        }
    }
}
