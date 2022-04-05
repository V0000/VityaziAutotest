using NUnit.Framework;
using System;
using Altom.AltUnityDriver;
using NUnit.Framework.Interfaces;
using System.Diagnostics;

namespace VityaziAutotest
{
    public class Tests
    {
        string screenShotPath = "E:/SCREENSHOTS/";

        private AltUnityDriver altUnityDriver;

        [OneTimeSetUp]
        public void SetUp()
        {
            altUnityDriver = new AltUnityDriver(); //initialization of driver
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            altUnityDriver.Stop(); //stop driver
        }

        [TearDown] // call after each test
        public void TearDown()
        {
                // check the execution status of the current test (called in [TearDown])
                if (TestContext.CurrentContext.Result.Outcome == ResultState.Ignored) // if the test is ignored and not executed(if "Assert.Ignore()") 
                {      
                    
                }
            if ((TestContext.CurrentContext.Result.Outcome == ResultState.Failure) || (TestContext.CurrentContext.Result.Outcome == ResultState.Error)) // test ended with an error
                {
                string error = TestContext.CurrentContext.Result.Message + "\n" + TestContext.CurrentContext.Result.StackTrace;// get message and stacktrace
                    altUnityDriver.GetPNGScreenshot(screenShotPath + TestContext.CurrentContext.Test.Name + ".png");                                                                                                              
            }
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Success) //test completed successfully
                {
                altUnityDriver.GetPNGScreenshot(screenShotPath + TestContext.CurrentContext.Test.Name + ".png");
            }
        }

        [Test]
        public void Test0GameIsLoaded()
        {
            Assert.AreEqual("CityMap", altUnityDriver.GetCurrentScene());

        }

        [Test]
        public void Test0Failed()
        {
            Assert.IsNull(1);

        }


        [Test]
        public void Test1AddMoneyInBank()
        {

            var bankButton = altUnityDriver.FindObject(By.PATH, "/UI/Gold");
            bankButton.Tap();

            var bankAddGoldButton0 = altUnityDriver.FindObject(By.PATH, "/BPlaces/GoldWindow/GoldStore/Panel/BodyPanel/Button");
            var bankAddGoldButton1 = altUnityDriver.FindObject(By.PATH, "/BPlaces/GoldWindow/GoldStore/Panel/BodyPanel/Button (1)");
            var bankAddGoldButton2 = altUnityDriver.FindObject(By.PATH, "/BPlaces/GoldWindow/GoldStore/Panel/BodyPanel/Button (2)");
            var bankRemoveGold = altUnityDriver.FindObject(By.PATH, "/BPlaces/GoldWindow/GoldStore/Panel/BodyPanel/Button (3)");
            var CloseBankButton = altUnityDriver.FindObject(By.PATH, "/BPlaces/GoldWindow/GoldStore/Panel/HatPanel/CloseButton");


            bankRemoveGold.Tap();
            bankAddGoldButton0.Tap();
            bankAddGoldButton1.Tap();
            for (int i = 0; i < 4; i++)
            {
                bankAddGoldButton2.Tap();
            }
            CloseBankButton.Tap();

            int gold = Int32.Parse(altUnityDriver.FindObject(By.PATH, "/UI/Gold/Text").GetText());

            Console.WriteLine(gold);
            Assert.AreEqual(gold, 460);

        }


        [Test]
        public void Test2BuildingConstruction()
        {


            string buildPlacePath = "/BPlaces/BuildPlace ({0})";
            string buildButtonPath = "/BPlaces/BuildPlace ({0})/BuildingStore/Panel/BodyPanel/Button";
            int countOfBuildPlaces = 23;

            for (int i = 0; i < countOfBuildPlaces; i++)
            {
                var buildPlace = altUnityDriver.FindObject(By.PATH, String.Format(buildPlacePath, i));
                buildPlace.Tap();
                var buildButton = altUnityDriver.FindObject(By.PATH, String.Format(buildButtonPath, i));
                buildButton.Tap();
            }


            var buildings = altUnityDriver.FindObjects(By.PATH, "//House(Clone)"); // все домики
            Console.WriteLine(buildings.Count);
            Assert.AreEqual(countOfBuildPlaces, buildings.Count);



        }

        [Test]
        public void Test3SwitchToKingdomMap()
        {
            string buttonToKingdomPATH = "/UI/ButtonToKingdom";
            var buttonToKingdom = altUnityDriver.FindObject(By.PATH, buttonToKingdomPATH);
            buttonToKingdom.Tap();

            Assert.AreEqual("KingdomMap", altUnityDriver.GetCurrentScene());
        }

        [Test]
        public void Test4MapOfKingdomIsGenerated()
        {
            int countOfTiles = 10000;

            var tiles = altUnityDriver.FindObjects(By.PATH, "/MapGenerator/forest1(Clone)"); // все домики
            Console.WriteLine(tiles.Count);
            Assert.AreEqual(countOfTiles, tiles.Count);

            string cityOnMapName = "Church";
            var cityOnMap = altUnityDriver.FindObject(By.NAME, cityOnMapName);
            Assert.NotNull(cityOnMap);
        }

        [Test]
        public void Test5SwipeMapOfKingdom()
        {
            var draggableArea = altUnityDriver.FindObject(By.PATH, "/Church");
            var initialPosition = draggableArea.getScreenPosition();
            int fingerId = altUnityDriver.BeginTouch(draggableArea.getScreenPosition());

            AltUnityVector2 newPosition = new AltUnityVector2(draggableArea.x + 30, draggableArea.y);
            altUnityDriver.MoveTouch(fingerId, newPosition);

            newPosition = new AltUnityVector2(draggableArea.x + 30, draggableArea.y + 30);
            altUnityDriver.MoveTouch(fingerId, newPosition);

            newPosition = new AltUnityVector2(draggableArea.x, draggableArea.y + 30);
            altUnityDriver.MoveTouch(fingerId, newPosition);

            newPosition = new AltUnityVector2(draggableArea.x, draggableArea.y);
            altUnityDriver.MoveTouch(fingerId, newPosition);

            altUnityDriver.EndTouch(fingerId);
            draggableArea = altUnityDriver.FindObject(By.PATH, "/Church");

            Trace.WriteLine(initialPosition);
            Trace.WriteLine(draggableArea.getScreenPosition());
            Assert.AreNotEqual(initialPosition, draggableArea.getScreenPosition());

        }

        [Test]
        public void Test6SwitchToCityMap()
        {
            string buttonToCityPATH = "/Canvas/ButtonToCity";
            var buttonToCity = altUnityDriver.FindObject(By.PATH, buttonToCityPATH);
            buttonToCity.Tap();

            Assert.AreEqual("CityMap", altUnityDriver.GetCurrentScene());
        }
    }
}