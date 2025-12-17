using System;
using System.Reflection;

namespace ConsoleApplication2
{
    internal class Program
    {
        public class Student
        {
            private string Name = "Сима";
            private int Age = 17;
        }
        
        public static void Main()
        {
            Student student = new Student();
            Type type = typeof(Student);

            FieldInfo[] fields = type.GetFields(
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            
            Console.WriteLine("Исходные значения:");
            foreach (FieldInfo field in fields)
            {
                Object value = field.GetValue(student);
                Console.WriteLine(value);
            }

            foreach (FieldInfo field in fields)
            {
                if (field.Name == "Name")
                    field.SetValue(student, "Daniil");
                else
                    field.SetValue(student, 22);
            }
            
            Console.WriteLine("New значения:");
            foreach (FieldInfo field in fields)
            {
                Object value = field.GetValue(student);
                Console.WriteLine(value);
            }
        }
    }
}