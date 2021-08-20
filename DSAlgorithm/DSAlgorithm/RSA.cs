using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography;


namespace DSAlgorithm
{
    public class RSA
    {
        private BigInteger p;
        private BigInteger q;
        private BigInteger n;
        private BigInteger eulerNumb;
        private BigInteger e;
        private BigInteger d;
        public RSA()
        {
            this.p = RandomInteger();
            do
            {
                this.q = RandomInteger();
            } while (p == q);
            this.n = p * q;
            this.eulerNumb = (p - 1)* (q - 1);
            this.e = Euklid_Algoritm(eulerNumb, -1);
            this.d = Euklid_Algoritm(eulerNumb, e);
            if (d < 0)
            {
                d += eulerNumb;
            }
        }
        public BigInteger GetN()
        {
            return n;
        }
        public BigInteger GetE()
        {
            return e;
        }
        public BigInteger GetD()
        {
            return d;
        }

        private BigInteger Euklid_Algoritm(BigInteger b2, BigInteger a)
        {
            BigInteger b = b2;
            BigInteger a1;
            BigInteger mod = -1;                                                            // promena mod slouzi pro swap, a pozdeji zbytek po deleni
            if (a == -1)
            {
                while (mod != 1)
                {
                    b = b2;
                    a = RandomIntegerBelow(b2);
                    while (a == b2)
                    {
                        a = RandomIntegerBelow(b2);
                        if (BigInteger.ModPow(a, 1, 2) == 0)
                        {
                            a = b2;
                        }
                    }
                    a1 = a;

                    while (true)
                    {
                        if (a > 0)
                        {
                            mod = b % a;
                            if (mod == 1)
                            {
                                return a1;
                            }
                            else
                            {
                                b = a;
                                a = mod;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                BigInteger x1 = 1;
                BigInteger x2 = 0;
                BigInteger temp;

                if (a > b)
                {
                    mod = a;
                    a = b;
                    b = mod;
                }
                while (true)
                {
                    if (a > 0)
                    {
                        mod = b % a;
                        if (a == 1)
                        {
                            return x1;
                        }
                        else
                        {
                            temp = x1;
                            x1 = x2 - ((b / a) * x1);
                            x2 = temp;
                            b = a;
                            a = mod;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return -1;
        }
        static private bool IsPrime(BigInteger numb)
        {
            double sqrt = Math.Exp(BigInteger.Log(numb) / 2);

            if (numb == 0 || numb == 1)
            {
                return false;
            }
            else if (numb % 2 == 0)
            {
                return false;
            }
            else
            {
                for (int a = 3; a <= sqrt; a += 2)
                {
                    if (numb % a == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        private BigInteger RandomIntegerBelow(BigInteger eulerNumb) // to generate a random number
        {
            byte[] bytes = new byte[4];
            BigInteger numb;
            do
            {
                var rng = new RNGCryptoServiceProvider();
                rng.GetBytes(bytes);

                numb = new BigInteger(bytes);
                numb = BigInteger.Abs(numb);
                numb %= eulerNumb;
                if (numb % 2 == 0)
                {
                    numb = BigInteger.Subtract(numb, 1);
                }
                rng.Dispose();
            }
            while (!IsPrime(numb));

            return numb;
        }

        private BigInteger RandomInteger() // to generate a random number
        {
            byte[] bytes = new byte[4];
            BigInteger numb;
            do
            {
                var rng = new RNGCryptoServiceProvider();
                rng.GetBytes(bytes);

                numb = new BigInteger(bytes);
                numb = BigInteger.Abs(numb);
                if (numb % 2 == 0)
                {
                    numb = BigInteger.Subtract(numb, 1);
                }
                rng.Dispose();
            }
            while (!IsPrime(numb));

            return numb;
        }
    }
}
