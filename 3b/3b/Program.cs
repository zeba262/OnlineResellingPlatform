using System;

namespace Finances
{
    // Base class following Liskov Substitution Principle
    public class PremiumMembership
    {
        protected double balance = 10000.0;
        protected double min_balance = 300;

        // Method to process payment using a specific payment method
        public virtual void Payment(double amount, PaymentMethod paymentMethod)
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

            // Use the PaymentMethod to process payment
            paymentMethod.Pay(amount);

            balance -= amount;

            if (balance < min_balance)
            {
                Console.WriteLine("Running below Minimum balance.");
                Console.WriteLine("Please maintain a minimum balance of {0}/-", min_balance);
            }
        }
    }

    // Derived class for Standard Membership
    public class StandardMembership : PremiumMembership
    {
        public override void Payment(double amount, PaymentMethod paymentMethod)
        {
            double tax = amount * 0.15;
            Console.WriteLine("Tax Amount: {0}", tax);
            amount += tax;

            base.Payment(amount, paymentMethod);
            Console.WriteLine("Standard Premium Membership Activated!!!!!");
        }
    }

    // Derived class for Exclusive Membership
    public class ExclusiveMembership : PremiumMembership
    {
        public override void Payment(double amount, PaymentMethod paymentMethod)
        {
            double tax = amount * 0.18;
            Console.WriteLine("Tax Amount: {0}", tax);
            amount += tax;

            base.Payment(amount, paymentMethod);
            Console.WriteLine("Exclusive Premium Membership Activated!!!!!");
        }
    }

    // Abstract base class for PaymentMethod
    public abstract class PaymentMethod
    {
        public abstract void Pay(double amount);
    }

    // Subclass for Card Payment
    public class CardPayment : PaymentMethod
    {
        public override void Pay(double amount)
        {
            Console.WriteLine("Processing card payment...");
            Console.WriteLine("Payment of {0} has been successfully processed using Card.", amount);
        }
    }

    // Subclass for Cash Payment
    public class CashPayment : PaymentMethod
    {
        public override void Pay(double amount)
        {
            Console.WriteLine("Processing cash payment...");
            Console.WriteLine("Payment of {0} has been successfully processed using Cash.", amount);
        }
    }

    public class LiskovSubstitutionDemo
    {
        public static void Main(string[] args)
        {

            Console.WriteLine("**Premium Membership**\n");
            Console.WriteLine("Choose a Plan.\n");
            Console.WriteLine("1) Standard Plan (Rs:500 + tax).");
            Console.WriteLine("2) Exclusive Plan (Rs:1020 + tax).");

            int n = Convert.ToInt32(Console.ReadLine());

            // Choose a Payment Method
            Console.WriteLine("Choose a Payment Method:\n1) Card\n2) Cash");
            int paymentChoice = Convert.ToInt32(Console.ReadLine());
            PaymentMethod paymentMethod = paymentChoice == 1 ? new CardPayment() : new CashPayment();

            PremiumMembership premiumMember;

            switch (n)
            {
                case 1:
                    premiumMember = new StandardMembership();
                    premiumMember.Payment(500, paymentMethod);
                    break;
                case 2:
                    premiumMember = new ExclusiveMembership();
                    premiumMember.Payment(1020, paymentMethod);
                    break;
                default:
                    Console.WriteLine("Invalid Option!!");
                    break;
            }
        }
    }
}