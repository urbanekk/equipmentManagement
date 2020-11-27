using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace equipmentMangement
{
    public partial class equipment
    {
        public List<string> res { get; set; }

        public void SprRes(DateTime start, DateTime stop)
        {
            res = new List<string>();
            bool isReservated;
            for(int i =0; i < (stop - start).TotalDays+1; i++)
            {
                isReservated = false;
                foreach(reservations res in this.reservations)
                {
                    isReservated = DataInRange(start.AddDays(i), (DateTime)res.StartDate, (DateTime)res.StopDate);
                    if (isReservated)
                        break;
                }
                
                if (isReservated)
                {
                    res.Add("REZERWACJA");
                    
                }
                else
                {
                    res.Add(" ");
                }


            }

        }

        private bool DataInRange(DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck >= startDate && dateToCheck <= endDate;
        }
    }
}
