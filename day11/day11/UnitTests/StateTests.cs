﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using day11;
using Xunit;

namespace UnitTests
{
	public class StateTests
	{
		private State testState;
		private State testStateStep2;
		private State emptyTestState;
		private const int F1 = 0;
		private const int F2 = 1;
		private const int F3 = 2;

		public StateTests()
		{
			//F3	.	.				XM	YG
			//F2	.	.	HM	LG	LM	.	.
			//F1	E	HG	.	.	.	.	.
			int floorCount = 3;
			int itemCount = 6 + 1;
			
			testState = new State(floorCount, itemCount);
			testState.AddItem(ItemTypes.Elevator, F1, null);
			testState.AddItem(ItemTypes.Generator, F1, 'H');
			testState.AddItem(ItemTypes.Microchip, F2, 'H');
			testState.AddItem(ItemTypes.Generator, F2, 'L');
			testState.AddItem(ItemTypes.Microchip, F2, 'L');
			testState.AddItem(ItemTypes.Microchip, F3, 'X');
			testState.AddItem(ItemTypes.Generator, F3, 'Y');

			testStateStep2 = new State(4, itemCount);
			testStateStep2.AddItem(ItemTypes.Elevator, F1, null);
			testStateStep2 = new State(floorCount, itemCount);

			testStateStep2.AddItem(ItemTypes.Generator, F2, 'H');
			testStateStep2.AddItem(ItemTypes.Microchip, F2, 'H');
			testStateStep2.AddItem(ItemTypes.Microchip, F1, 'L');
			
			emptyTestState = new State(1, 1);

		}


		[Fact]
		public void FloorItemsTests()
		{
			var firstFloorItems =  testState.FloorItems(F1);
			Assert.Equal(1 , firstFloorItems.Count);
			Assert.Contains("HG", firstFloorItems);
			
			var secondFloorItems = testState.FloorItems(F2);
			Assert.Equal(3, secondFloorItems.Count);
			Assert.Contains("HM", secondFloorItems);
			Assert.Contains("LG", secondFloorItems);
			Assert.Contains("LM", secondFloorItems);
			Assert.DoesNotContain("HG", secondFloorItems);
		}

		[Fact]
		public void ElevatorFloorTests()
		{
			Assert.Equal(F1, testState.ElevatorFloor());
			var noElevatorState = new State(1, 1);
			Assert.Equal(-1, noElevatorState.ElevatorFloor());
			testState = null;
		}

		[Fact]
		public void PairedItemTests()
		{
			Assert.Equal($"H{(char)ItemTypes.Generator}", State.PairedItemName($"H{(char)ItemTypes.Microchip}"));
			Assert.Equal($"X{(char)ItemTypes.Microchip}", State.PairedItemName($"X{(char)ItemTypes.Generator}"));
		}

		[Fact]
		public void ItemTypeTests()
		{
			Assert.Equal(ItemTypes.Generator, State.ItemType($"H{(char)ItemTypes.Generator}"));
			Assert.Equal(ItemTypes.Microchip, State.ItemType($"X{(char)ItemTypes.Microchip}"));
			Assert.Equal(ItemTypes.Elevator, State.ItemType($"{(char)ItemTypes.Elevator}"));
		}

		[Fact]
		public void IsFloorSafeTests()
		{
			Assert.True(testState.IsFloorSafe(F1));
			Assert.False(testState.IsFloorSafe(F2));
			Assert.False(testState.IsFloorSafe(F3));
			Assert.True(emptyTestState.IsFloorSafe(F1));

			Assert.True(testStateStep2.IsFloorSafe(F1));
			Assert.True(testStateStep2.IsFloorSafe(F2));
			Assert.True(testStateStep2.IsFloorSafe(F3));
		}

		[Fact]
		public void IsSafeTests()
		{
			Assert.False(testState.IsSafe());
			Assert.True(testStateStep2.IsSafe());
			Assert.True(emptyTestState.IsSafe());
		}

		[Fact]
		public void IsDoneTests()
		{
			Assert.False(testState.isSolved());
			Assert.False(emptyTestState.isSolved());
		}


		[Fact]
		public void MoveItemsTests()
		{
			var firstFloorItems = testState.FloorItems(F1);
			Assert.Equal(1, firstFloorItems.Count);
			Assert.Contains("HG", firstFloorItems);
			var secondFloorItems = testState.FloorItems(F2);
			Assert.Equal(3, secondFloorItems.Count);
			Assert.DoesNotContain("HG", secondFloorItems);
			Assert.Equal(F1, testState.ElevatorFloor());

			var newState = testState.MoveItems(new List<string> {"HG"}, F2);
			
			firstFloorItems = newState.FloorItems(F1);
			Assert.Equal(0, firstFloorItems.Count);
			Assert.DoesNotContain("HG", firstFloorItems);
			secondFloorItems = newState.FloorItems(F2);
			Assert.Equal(4, secondFloorItems.Count);
			Assert.Contains("HG", secondFloorItems);
			Assert.Equal(F2, newState.ElevatorFloor());

		}
	}
}