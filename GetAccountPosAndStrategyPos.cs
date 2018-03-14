#region Using declarations
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Indicator;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Strategy;
using System.IO;
using System.Collections.Generic;
#endregion

// https://ichimoku-expert.blogspot.com
namespace NinjaTrader.Strategy
{
    [Description("Get account positions and strategy position")]
    public class GLStrategy : Strategy
    {
        protected override void Initialize()
        {
			this.BarsRequired = 1;
            CalculateOnBarClose = false;
			this.ClearOutputWindow();
			checkParamFile();
        }

		double ask = 0;
		double bid = 0;
		double PnL = 0;
		DateTime dt = new DateTime();
		bool done = false;
        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
			dt = DateTime.Now;	
			
			Print("**** Account Positions ****");
			if (Account.Positions.Count>0)
			{
				for (int i=0;i<Account.Positions.Count;i++)
				{
					try
					{
						Print("Account Position " + i);
						Print(">> " + dt + " : Instrument Name = " + Account.Positions[i].Instrument.FullName);
						Print(">> " + dt + " : PnL = " + Account.Positions[i].GetProfitLoss(GetCurrentBid(), PerformanceUnit.Currency));
						Print(">> " + dt + " : Qty = " + Account.Positions[i].Quantity);
						/*double profit = Account.Positions[i].GetProfitLoss(GetCurrentBid(), PerformanceUnit.Currency);
						if (profit>=100)
						{
							Account.Positions[i].Close();
						}*/
					}
					catch(Exception ex)
					{
						Print("Exception while trying to access Account Position " + i + " : " + ex.Message);
					}
				}
			}
			Print("**** Strategy Position ****");
			PnL = Position.GetProfitLoss(GetCurrentBid(), PerformanceUnit.Currency);
			Print(">> " + dt + " : PnL =" + PnL);
			Print(">> " + dt + " : Qty =" + Position.Quantity);
						
			if (this.Historical == true) return;
			
			double ask = this.GetCurrentAsk();
			double bid = this.GetCurrentBid();
			if (done == false)
			{
				//Print("Will enter long position");
				//IOrder iorder = EnterLong(100000);
				done = true;
			}			
			
			Print("");
        }
		
    }
}
