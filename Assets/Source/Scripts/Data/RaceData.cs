using System;

namespace Source.Scripts.Data
{
    public class RaceData
    {
        public event Action<int> OnMoneyChanged; 
        
        public int Money { get; private set; }
        
        public void AddMoney(int reward)
        {   
            if(reward < 0)
            {
                throw new ArgumentException("Reward cannot be negative");
            }
            
            Money += reward;
            OnMoneyChanged?.Invoke(Money);
        }
    }
}