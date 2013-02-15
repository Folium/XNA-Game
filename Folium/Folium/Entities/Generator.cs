using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Screens;
using Folium.Main;

namespace Folium.Entities
{
	/*
		Generates the entities in the world, both at the start of the game and as the game progresses.
	*/
	class Generator
	{
		protected GameManager _gameManager;
        protected Screen _screen;

		private List<Type> _types;
		private List<float> _amounts;

		public Generator(GameManager gameManager, Screen screen) {
			_gameManager = gameManager;
			_screen = screen;

			_types = new List<Type>(32);
			_amounts = new List<float>(32);
		}

		/*
			Adds a type of entity to the generator.
		*/
		public void addType(Type type, float amount) {
			Console.WriteLine(type);
			_types.Add(type);
			_amounts.Add(amount);
		}

		/*
			Initializes the starting world.
		*/
		public void initialize() {
			// Create the specified amount for eacht type.
			for(int i = 0; i < _types.Count; i++) {
				for (int j = 0; j < _amounts[i]; j++) {
					Food pickup = (Food)Activator.CreateInstance(_types[0], _gameManager, _screen);
				}
			}
		}

		/*
			Generate entities as the organism grows near the boundaries of the screen
		*/
		public void update()
		{

		}
	}
}
