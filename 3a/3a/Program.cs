using System;

namespace Finances
{
    interface IPayment
    {
        void Payment(double amount);
    }

    public class PremiumMembership : IPayment
    {
        private double balance = 1000;
        private double min_balance = 300;

        public virtual void Payment(double amount)
        {
            if (balance > 0)
            {
                if (amount <= 0)
                {
                    Console.WriteLine("Invalid Amount");
                    return;
                }
                else if (amount > balance)
                {
                    Console.WriteLine("Insufficient Balance!");
                    return;
                }

                double tax = amount * 0.10;
                Console.WriteLine("Tax Amount: {0}", tax);
                amount += tax;
                Console.WriteLine("Amount inclusive of Taxes: {0}", amount);
                balance -= amount;
            }

            if (balance < min_balance)
            {
                Console.WriteLine("Running below Minimum balance.");
                Console.WriteLine("Please maintain a minimum balance of {0}/-", min_balance);
            }
        }
    }


    public class LiskovSubstitutionDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("**Premium MemberShip**");
            Console.WriteLine("Choose a Plan.");
            Console.WriteLine("1) Standard Plan(rs:500 + tax).");
            Console.WriteLine("2) Exclusive Plan(rs:1020 + tax).");
            int n = Convert.ToInt32(Console.ReadLine());
            IPayment premiumMember = new PremiumMembership();

            switch (n)
            {
                case 1:
                    premiumMember.Payment(500);
                    break;
                case 2:
                    premiumMember.Payment(1020);
                    break;
                default:
                    Console.WriteLine("Invalid Option!!");
                    break;
            }

        }
    }
} 