using System;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementApp
    {
        
        public class Feet
        {
            private readonly double value;

            public Feet(double value)
            {
                this.value = value;
            }

            public override bool Equals(object obj)
            {
                // 1. Reference Check If both references point to the same object
                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                // 2. Null Check If the compared object is null
                if (obj is null) 
                {
                    return false;
                }

                // 3. Type Check If the compared object is not of type Feet
                if (GetType() != obj.GetType())
                {
                    return false;
                }

                // 4. Value Comparison compare the double values for equality
                Feet otherFeet = (Feet)obj;
                return value.CompareTo(otherFeet.value) == 0;
            }
            public override int GetHashCode()
            {
                return value.GetHashCode();
            }
        }
        public static void Main(string[] args)
        {
            Feet feet1 = new Feet(10.0);
            Feet feet2 = new Feet(10.0);
            Feet feet3 = new Feet(5.0);
            Feet feet4 = feet1;

            Console.WriteLine("feet1 equals feet2: " + feet1.Equals(feet2)); 
            Console.WriteLine("feet1 equals feet3: " + feet1.Equals(feet3)); 
            Console.WriteLine("feet1 equals feet4: " + feet1.Equals(feet4)); // Should be true
            Console.WriteLine("feet1 equals null: " + feet1.Equals(null));   
            Console.WriteLine("feet1 equals new object : " + feet1.Equals(new object())); // Should be false

        }
    }
}