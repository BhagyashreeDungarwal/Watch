namespace Watch.Utility
{
    public class SD
    {
        //add role for individual user
        public const string Role_User_Indi = "Individual";
        //add role for company useer
        public const string Role_User_Comp = "Company";
        //add role for MAnagement side and the company side users
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";

        //Add For Order status
        public const string StatusPending = "Pending";//Order Created
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        //Payment Status
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
        public const string PaymentStatusREjected = "Rejected";
    }
}