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
#endregion

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    /// <summary>
    /// https://ichimoku-expert.blogspot.com
    /// </summary>
    [Description("https://ichimoku-expert.blogspot.com")]
    public class BuyStopOrderWithSLandTP_Strategy : Strategy
    {
        #region Variables
        // Wizard generated variables
        private int myInput0 = 1; // Default setting for MyInput0
        // User defined variables (add any user defined variables below)
        #endregion

        /// <summary>
        /// This method is used to configure the strategy and is called once before any strategy method is called.
        /// </summary>
        protected override void Initialize()
        {
            this.BarsRequired = 0;
            CalculateOnBarClose = false;
            this.EntriesPerDirection = 8;
			      this.BarsRequired = 0;
        }

		protected override void OnStartUp()
        {
			this.ClearOutputWindow();
			this.checkParamFile();
		}
		
		double ask = 0;
		double bid = 0;
		bool done = false;
        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
			ask = this.GetCurrentAsk();
            bid = this.GetCurrentBid();
			
			if (this.Historical == true) return;
			
			if (done == false){
				IOrder order = this.EnterLongStop(1, ask+5, "MonSignalName");
				this.SetProfitTarget("MonSignalName", CalculationMode.Price, bid+10);
				this.SetStopLoss("MonSignalName", CalculationMode.Price, ask-20, false);
				Print("stop price = " + (ask+5));
				Print("tp price = " + (bid+10));
				Print("sl price = " + (ask-20));
				done = true;
			}
        }
		
        #region Properties
        [Description("")]
        [GridCategory("Parameters")]
        public int MyInput0
        {
            get { return myInput0; }
            set { myInput0 = Math.Max(1, value); }
        }
        #endregion
    }
}
