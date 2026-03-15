using System;
using Microsoft.Extensions.Configuration;

namespace QuantityMeasurementApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IQuantityMenu menu= AppConfiguration.CreateMenuFromConfig();
            menu.ShowMainMenu();

        }
    }
}