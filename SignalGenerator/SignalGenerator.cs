using System;
using System.Diagnostics;
using MicroLibrary;

namespace Signals
{
    public class SignalGenerator
    {
        #region [ Properties ... ]

        /// <summary>
        /// Signal Type.
        /// </summary>
        /// 
        public double vmedio
        {
            get
            {

                double value = offset;

                if (v_amostras == null)
                {
                    switch (signalType)
                    {
                        case SignalType.Sawtooth:
                            value += ((1 / 2) * ((1 / frequency) * (1 / 2)) * amplitude - (1 / 2) * ((1 / frequency) * (1 / 2)) * amplitude) / (1 / frequency);
                            break;

                    }
                }
                else
                {
                    value = 0;
                    for (int i = 0; i < v_amostras.Length; i++)
                    {
                        value += v_amostras[i];
                    }
                    value /= v_amostras.Length;
                    value += offset;
                }

                return value;
            }
        }


        public SignalType SignalType
        {
            get { return signalType; }
            set { signalType = value; }
        }


        public double tempoamostra
        {
            get { return tamostra; }
        }

        public int numamostras
        {
            get { return namostras; }
        }


        private double frequency = 1f;
        /// <summary>
        /// Signal Frequency.
        /// </summary>
        public double Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }
        public double FrequencyMult
        {
            get { return freq_mult; }
            set { freq_mult = value; }
        }

        private double phase = 0f;
        /// <summary>
        /// Signal Phase.
        /// </summary>
        public double Phase
        {
            get { return phase; }
            set { phase = value; }
        }

        private double amplitude = 1f;
        /// <summary>
        /// Signal Amplitude.
        /// </summary>
        public double Amplitude
        {
            get
            {
                if (signalType != SignalType.ExcelCSV)
                {
                    return amplitude;
                }
                else
                {
                    double max = v_amostras[0];

                    for (int n = 1; n < v_amostras.Length; n++)
                    {
                        if (max > v_amostras[n])
                        {
                            max = v_amostras[n];
                        }
                    }
                    return max;
                }
            }
            set
            {
                if (signalType != SignalType.ExcelCSV)
                {
                    amplitude = value;
                }
            }

        }

        private double offset = 0f;
        /// <summary>
        /// Signal Offset.
        /// </summary>
        public double Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        private double invert = 1; // Yes=-1, No=1
        /// <summary>
        /// Signal Inverted?
        /// </summary>
        public bool Invert
        {
            get { return invert == -1; }
            set { invert = value ? -1 : 1; }
        }

        private GetValueDelegate getValueCallback = null;
        /// <summary>
        /// GetValue Callback?
        /// </summary>
        public GetValueDelegate GetValueCallback
        {
            get { return getValueCallback; }
            set { getValueCallback = value; }
        }

        #endregion  [ Properties ]

        #region [ Private ... ]
        private SignalType signalType = SignalType.Sine;
        private double tamostra;
        private int namostras;
        //        private double[] vals;

        /// <summary>
        /// Random provider for noise generator
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// Time the signal generator was started
        /// </summary>
        protected long startTime = MicroStopwatch.GetTimestamp();

        /// <summary>
        /// Ticks per second on this CPU
        /// </summary>
        protected long ticksPerSecond = MicroStopwatch.Frequency;

        #endregion  [ Private ]


        #region [ Public ... ]

        private double[] v_amostras;
        private double freq_mult = 1;
        private SignalGenerator sig2 = null;
        private OperType op;

        public double amostra(double t)
        {
            //int n = Convert.ToInt32(t / tamostra);
            //n=n>=vals.Length?vals.Length-1:n;
            double res;
            if (signalType == SignalType.ExcelCSV)
            {
                int n = Convert.ToInt32(t / tamostra);
                while (n < 0)
                {
                    n += v_amostras.Length;
                }
                n %= v_amostras.Length;
                res = v_amostras[n];

            }
            else
            {
                res = GetValue(t);
            }

            if (sig2 != null)
            {
                switch (op)
                {
                    case OperType.Sum:
                        res += sig2.amostra(t);
                        break;
                    case OperType.Sub:
                        res -= sig2.amostra(t);
                        break;
                    case OperType.Mult:
                        res *= sig2.amostra(t);
                        break;
                    case OperType.Div:
                        res /= sig2.amostra(t);
                        break;
                }
            }

            return res;

        }



        public delegate double GetValueDelegate(double time);

        public SignalGenerator(SignalType initialSignalType)
        {
            signalType = initialSignalType;
        }

        public SignalGenerator(SignalType type, double[] am, double tamostra)
        {
            signalType = type;
            this.v_amostras = am;
            this.tamostra = tamostra;
        }

        public SignalGenerator(SignalType type, double[] am, double tamostra, double freq, double ampl)
        {
            signalType = type;
            this.v_amostras = am;
            this.tamostra = tamostra;
            this.frequency = freq;
            this.amplitude = ampl;
        }


        public SignalGenerator(SignalType type, double offs)
        {
            signalType = type;
            this.offset = offs;
            this.amplitude = 1;
            if (signalType == SignalType.DC)
            {
                this.frequency = 0;
            }
        }

        public SignalGenerator(SignalType type, double freq, double ampl, double offs, int nper, int namos)
        {
            signalType = type;
            this.tamostra = ((double)nper / freq) / (double)namos;
            this.namostras = namos;
            this.amplitude = ampl;
            this.frequency = freq;
            this.offset = offs;

            /*
            vals = new double[this.namostras];
            double t = 0;

            for (int i = 0; i < vals.Length; i++)
            {
                vals[i] = GetValue(t);
                t += this.tamostra;
            }*/

        }

        public SignalGenerator(SignalType type, double freq, double ampl, double offs, double tamostra, double fm)
        {
            signalType = type;
            this.tamostra = tamostra;
            this.amplitude = ampl;
            this.frequency = freq;
            this.offset = offs;
            this.freq_mult = fm;

        }

        public SignalGenerator() { }





#if DEBUG
        public double GetValue(double time)
#else
			private double GetValue(double time)
#endif
        {
            double value = 0f;
            double t = frequency * time + phase;
            switch (signalType)
            { // http://en.wikipedia.org/wiki/Waveform
                case SignalType.DC:
                    value = 0;
                    break;
                case SignalType.Sine: // sin( 2 * pi * t )
                    value = (double)Math.Sin(2f * Math.PI * t);
                    break;
                case SignalType.Square: // sign( sin( 2 * pi * t ) )
                    value = Math.Sign(Math.Sin(2f * Math.PI * t));
                    break;
                case SignalType.Triangle: // 2 * abs( t - 2 * floor( t / 2 ) - 1 ) - 1
                    value = 1f - 4f * (double)Math.Abs(Math.Round(t - 0.25f) - (t - 0.25f));
                    break;
                case SignalType.Sawtooth: // 2 * ( t/a - floor( t/a + 1/2 ) )
                    value = 2f * (t - (double)Math.Floor(t + 0.5f));
                    break;

                /*
                 case SignalType.UserDefined:
                    value = (getValueCallback==null) ? (0f): getValueCallback(t);
                    break;*/
            }

            return (invert * amplitude * value + offset);
        }

        public double GetValue()
        {
            double time = (double)(MicroStopwatch.GetTimestamp() - startTime) / ticksPerSecond;
            return GetValue(time);
        }

        public void Reset()
        {
            startTime = MicroStopwatch.GetTimestamp();
        }

        public void Synchronize(SignalGenerator instance)
        {
            startTime = instance.startTime;
            ticksPerSecond = instance.ticksPerSecond;
        }

        public void setOper(OperType op, SignalGenerator s)
        {
            this.sig2 = s;
            this.op = op;
        }

        public void unsetOper()
        {
            this.sig2 = null;
        }


        #endregion [ Public ]
    }

    #region [ Enums ... ]

    public enum SignalType
    {
        DC,
        Sine,
        Square,
        Triangle,
        Sawtooth,
        ExcelCSV

        /*,
		
        UserDefined */
        // user defined between -1 and 1	}
    }

    public enum OperType
    {
        Sum, Sub, Mult, Div
    }

    #endregion [ Enums ]

    #region [ Statistic ... ]

    public class StatisticFunction
    {
        // http://geeks.netindonesia.net/blogs/anwarminarso/archive/2008/01/13/normsinv-function-in-c-inverse-cumulative-standard-normal-distribution-function.aspx
        // http://home.online.no/~pjacklam/notes/invnorm/impl/misra/normsinv.html

        public static double Mean(double[] values)
        {
            double tot = 0;
            foreach (double val in values)
                tot += val;

            return (tot / values.Length);
        }

        public static double StandardDeviation(double[] values)
        {
            return Math.Sqrt(Variance(values));
        }

        public static double Variance(double[] values)
        {
            double m = Mean(values);
            double result = 0;
            foreach (double d in values)
                result += Math.Pow((d - m), 2);

            return (result / values.Length);
        }

        //
        // Lower tail quantile for standard normal distribution function.
        //
        // This function returns an approximation of the inverse cumulative
        // standard normal distribution function.  I.e., given P, it returns
        // an approximation to the X satisfying P = Pr{Z <= X} where Z is a
        // random variable from the standard normal distribution.
        //
        // The algorithm uses a minimax approximation by rational functions
        // and the result has a relative error whose absolute value is less
        // than 1.15e-9.
        //
        // Author:      Peter J. Acklam
        // (Javascript version by Alankar Misra @ Digital Sutras (alankar@digitalsutras.com))
        // Time-stamp:  2003-05-05 05:15:14
        // E-mail:      pjacklam@online.no
        // WWW URL:     http://home.online.no/~pjacklam

        // An algorithm with a relative error less than 1.15*10-9 in the entire region.

        public static double NORMSINV(double p)
        {
            // Coefficients in rational approximations
            double[] a = {-3.969683028665376e+01,  2.209460984245205e+02,
				-2.759285104469687e+02,  1.383577518672690e+02,
				-3.066479806614716e+01,  2.506628277459239e+00};

            double[] b = {-5.447609879822406e+01,  1.615858368580409e+02,
				-1.556989798598866e+02,  6.680131188771972e+01,
				-1.328068155288572e+01 };

            double[] c = {-7.784894002430293e-03, -3.223964580411365e-01,
				-2.400758277161838e+00, -2.549732539343734e+00,
				4.374664141464968e+00,  2.938163982698783e+00};

            double[] d = { 7.784695709041462e-03,  3.224671290700398e-01,
				2.445134137142996e+00,  3.754408661907416e+00};

            // Define break-points.
            double plow = 0.02425;
            double phigh = 1 - plow;

            // Rational approximation for lower region:
            if (p < plow)
            {
                double q = Math.Sqrt(-2 * Math.Log(p));
                return (((((c[0] * q + c[1]) * q + c[2]) * q + c[3]) * q + c[4]) * q + c[5]) /
                    ((((d[0] * q + d[1]) * q + d[2]) * q + d[3]) * q + 1);
            }

            // Rational approximation for upper region:
            if (phigh < p)
            {
                double q = Math.Sqrt(-2 * Math.Log(1 - p));
                return -(((((c[0] * q + c[1]) * q + c[2]) * q + c[3]) * q + c[4]) * q + c[5]) /
                    ((((d[0] * q + d[1]) * q + d[2]) * q + d[3]) * q + 1);
            }

            // Rational approximation for central region:
            {
                double q = p - 0.5;
                double r = q * q;
                return (((((a[0] * r + a[1]) * r + a[2]) * r + a[3]) * r + a[4]) * r + a[5]) * q /
                    (((((b[0] * r + b[1]) * r + b[2]) * r + b[3]) * r + b[4]) * r + 1);
            }
        }


        public static double NORMINV(double probability, double mean, double standard_deviation)
        {
            return (NORMSINV(probability) * standard_deviation + mean);
        }

        public static double NORMINV(double probability, double[] values)
        {
            return NORMINV(probability, Mean(values), StandardDeviation(values));
        }

    }
    #endregion [ Statistic ]

}

