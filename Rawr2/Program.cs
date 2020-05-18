using System;

namespace Rawr2
{
    class Program
    {
        static void Main(string[] args)
        {
            RetriCompare();
        }

        public static void RetriCompare()
        {
            var totalWf = 0d;
            var totalNoWf = 0d;
            var tries = 100;

            for (var i = 0; i < tries; i++)
            {
                totalWf += Retri();
                totalNoWf += Retri(false);
            }

            Console.WriteLine($"");
            Console.WriteLine($"WF");
            Console.WriteLine($"Number of tries: {tries}x");
            Console.WriteLine($"AVG: {totalWf / tries} DMG; {totalWf / tries / 120} DPS");
            Console.WriteLine($"");
            Console.WriteLine($"No WF");
            Console.WriteLine($"Number of tries: {tries}x");
            Console.WriteLine($"AVG: {totalNoWf / tries} DMG; {totalNoWf / tries / 120} DPS");
            Console.ReadKey();
        }

        public static double Retri(bool isWf = true)
        {
            var random = new Random();

            var missChance = 100;
            var wfChance = 2000;
            var critChance = 3142;
            var wfAdd = 160.71;

            var meleeDmg = 1297;
            var crusaderStrikeDmg = 1348;
            var sealDmg = 472;
            var judgementDmg = 436;

            var autoAttackCount = 33;
            var judgementCount = 15;
            var crusaderStrikeCount = 20;

            var autoAttackTotalDmg = 0d;
            var sealTotalDmg = 0d;
            var crusaderTotalDmg = 0d;
            var judgementTotalDmg = 0d;

            var extraWfProcs = 0;
            var extraWfAttackDmg = 0d;
            var extraWfSealDmg = 0d;

            for (var i = 0; i < autoAttackCount; i++)
            {
                if (IsProc(random, missChance, "Dodged autoattack"))
                {
                    continue;
                }

                if (isWf)
                {
                    if (IsProc(random, wfChance, "Windfury"))
                    {
                        extraWfProcs++;

                        var extraSealDmg = IsProc(random, critChance, "Seal of Blood crit") ? (meleeDmg + wfAdd) / 0.37 * 2 : (meleeDmg + wfAdd) / 0.37;
                        sealTotalDmg += extraSealDmg;
                        extraWfSealDmg += extraSealDmg;

                        var extraAutoattackDmg = IsProc(random, critChance, "WF AutoAttack crit") ? (meleeDmg + wfAdd) * 2 : meleeDmg + wfAdd;
                        autoAttackTotalDmg += extraAutoattackDmg;
                        extraWfAttackDmg += extraAutoattackDmg;
                    }
                }

                autoAttackTotalDmg += IsProc(random, critChance, "AutoAttack crit") ? meleeDmg * 2 : meleeDmg;
                sealTotalDmg += IsProc(random, critChance, "Seal of Blood crit") ? sealDmg * 2 : sealDmg;
            }

            for (var i = 0; i < crusaderStrikeCount; i++)
            {
                crusaderTotalDmg += IsProc(random, critChance, "Crusader Strike crit") ? crusaderStrikeDmg * 2 : crusaderStrikeDmg;
            }

            for (var i = 0; i < judgementCount; i++)
            {
                judgementTotalDmg += IsProc(random, critChance, "Judgement crit") ? judgementDmg * 2 : judgementDmg;
            }

            var totalDmg = autoAttackTotalDmg + crusaderTotalDmg + judgementTotalDmg + sealTotalDmg;

            LogResult(crusaderStrikeDmg, judgementDmg, autoAttackTotalDmg, sealTotalDmg, extraWfAttackDmg, extraWfSealDmg, totalDmg);

            return totalDmg;
        }

        private static bool IsProc(Random random, int chance, string action)
        {
            var rng = random.Next(0, 10000);

            if (chance >= rng)
            {
                //Console.WriteLine(action);
            }

            return chance >= rng;
        }

        private static void LogResult(int crusaderStrikeDmg, int judgementDmg, double autoAttackTotalDmg,
            double sealTotalDmg, double extraWfAttackDmg, double extraWfSealDmg, double totalDmg)
        {
            Console.WriteLine($"Retri");
            Console.WriteLine($"AutoAttack - {autoAttackTotalDmg}");
            Console.WriteLine($"Crusader Strike - {crusaderStrikeDmg}");
            Console.WriteLine($"SoB - {sealTotalDmg}");
            Console.WriteLine($"JSoB - {judgementDmg}");
            Console.WriteLine($"-----------");
            Console.WriteLine($"WF section");
            Console.WriteLine($"-----------");
            Console.WriteLine($"AutoAttack - {extraWfAttackDmg}");
            Console.WriteLine($"SoB - {extraWfSealDmg}");
            Console.WriteLine($"-----------");
            Console.WriteLine($"Total");
            Console.WriteLine($"-----------");
            Console.WriteLine($"{totalDmg} DMG, {totalDmg / 120} DPS");
        }
    }
}
