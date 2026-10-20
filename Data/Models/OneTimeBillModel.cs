using System.ComponentModel;

namespace BudgetWPF.Data.Models
{
    public class OneTimeBillModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int? ParentId { get; private set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsChanged { get; set; } = false;
        public int Id { get; private set; } = -1;
        public string Name { 
            get; 
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged("Name");
                }
            } 
        }
        public decimal Amount { 
            get; 
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged("Amount");
                }
            }
        }
        public DateOnly DueDate { 
            get; 
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged("DueDate");
                }
            }
        }
        public bool IsPaid { 
            get; 
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged("IsPaid");
                }
            }
        }
        public OneTimeBillModel(string name, decimal amount, DateOnly dueDate, bool isPaid)
        {
            Id = -1;
            Name = name;
            Amount = amount;
            DueDate = dueDate;
            IsPaid = isPaid;
            ParentId = null;
        }

        public OneTimeBillModel(string name, decimal amount, DateOnly dueDate, bool isPaid, int parentId)
        {
            Id = -1;
            Name = name;
            Amount = amount;
            DueDate = dueDate;
            IsPaid = isPaid;
            ParentId = parentId;
        }

        public OneTimeBillModel(int id, string name, decimal amount, DateOnly dueDate, bool isPaid, int? parentId)
        {
            Id = id;
            Name = name;
            Amount = amount;
            DueDate = dueDate;
            IsPaid = isPaid;
            ParentId = parentId;
        }

        public void MarkForDeletion()
        {
            IsDeleted = true;
        }

        public void TogglePaid()
        {
            IsPaid = !IsPaid;
        }

        public override string ToString()
        {
            string result = $"Id: {Id}";
            result += $"\nParent Id: {ParentId}";
            result += $"\nName: {Name}";
            result += $"\nAmount: {Amount}";
            result += $"\nDueDate: {DueDate}";
            result += $"\nIsPaid: {IsPaid}";
            return result;
        }

        private void OnPropertyChanged(string propertyName)
        {
            IsChanged = true;
            PropertyChangedEventArgs eventArgs = new(propertyName);
            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}