using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantApp.Core.Entity
{
    public enum PaymentOrderState
    {
        Draft,
        Approved,
        Rejected,
        Cancelled
    }

    public enum PaymentOrderTransition
    {
        Approve,
        Reject,
        Cancel
    }

    public class PaymentOrder
    {
        public int Id { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public string PaymentItems { get; set; }
        public PaymentOrderState State { get; set; } = PaymentOrderState.Draft;
        public double TotalPrice { get; set; }

        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        private Dictionary<AccountType, List<(PaymentOrderState, PaymentOrderTransition, PaymentOrderState)>> transitions = new Dictionary<AccountType, List<(PaymentOrderState, PaymentOrderTransition, PaymentOrderState)>>
        {
            [AccountType.Restaurant] = new List<(PaymentOrderState, PaymentOrderTransition, PaymentOrderState)>
            {
                (PaymentOrderState.Draft, PaymentOrderTransition.Approve, PaymentOrderState.Approved),
                (PaymentOrderState.Draft, PaymentOrderTransition.Reject, PaymentOrderState.Rejected),
            },
            [AccountType.User] = new List<(PaymentOrderState, PaymentOrderTransition, PaymentOrderState)>
            {
                (PaymentOrderState.Draft, PaymentOrderTransition.Cancel, PaymentOrderState.Cancelled)
            }
        };

        public List<string> GetAvailableTransitions(AccountType accountType)
        {
            return transitions[accountType].FindAll(t => t.Item1.ToString() == State.ToString()).Select(t => t.Item2.ToString()).ToList();
        }

        public void MakeTransition(AccountType accountType, string transition)
        {
            var avList = transitions[accountType].FindAll(t => t.Item1.ToString() == State.ToString());

            if (avList.Any(x => x.Item2.ToString() == transition)){
                State = avList.Find(x => x.Item2.ToString() == transition).Item3;
            }
            else
            {
                throw new InvalidOperationException($"Specified transition {transition} is not available");
            }
        }
    }
}
