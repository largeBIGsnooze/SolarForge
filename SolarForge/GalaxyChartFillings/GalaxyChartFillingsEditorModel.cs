using System;
using System.Collections.Generic;
using Solar.Scenarios;

namespace SolarForge.GalaxyChartFillings
{

	public class GalaxyChartFillingsEditorModel
	{



		public event GalaxyChartFillingsEditorModel.FillingsSourcesChangedDelegate FillingsSourcesChanged;




		public event GalaxyChartFillingsEditorModel.SelectedFillingsSourceChangedDelegate SelectedFillingsSourceChanged;




		public event GalaxyChartFillingsEditorModel.SelectedFillingsComponentChangedDelegate SelectedFillingsComponentChanged;




		public event GalaxyChartFillingsEditorModel.FixtureFillingsChangedDelegate FixtureFillingsChanged;




		public IEnumerable<GalaxyChartFillingsSource> FillingsSources
		{
			get
			{
				return this.fillingSources;
			}
			set
			{
				this.fillingSources = value;
				GalaxyChartFillingsEditorModel.FillingsSourcesChangedDelegate fillingsSourcesChanged = this.FillingsSourcesChanged;
				if (fillingsSourcesChanged == null)
				{
					return;
				}
				fillingsSourcesChanged();
			}
		}




		public GalaxyChartFillingsSource SelectedFillingsSource
		{
			get
			{
				return this.selectedFillingsSource;
			}
			set
			{
				this.selectedFillingsSource = value;
				GalaxyChartFillingsEditorModel.SelectedFillingsSourceChangedDelegate selectedFillingsSourceChanged = this.SelectedFillingsSourceChanged;
				if (selectedFillingsSourceChanged != null)
				{
					selectedFillingsSourceChanged();
				}
				this.SelectedFillingsComponent = null;
			}
		}




		public object SelectedFillingsComponent
		{
			get
			{
				return this.selectedFillingsComponent;
			}
			set
			{
				this.selectedFillingsComponent = value;
				GalaxyChartFillingsEditorModel.SelectedFillingsComponentChangedDelegate selectedFillingsComponentChanged = this.SelectedFillingsComponentChanged;
				if (selectedFillingsComponentChanged == null)
				{
					return;
				}
				selectedFillingsComponentChanged();
			}
		}



		public RandomSkyboxFilling SelectedRandomSkyboxFilling
		{
			get
			{
				return this.selectedFillingsComponent as RandomSkyboxFilling;
			}
		}



		public RandomFixtureFilling SelectedRandomFixtureFilling
		{
			get
			{
				return this.selectedFillingsComponent as RandomFixtureFilling;
			}
		}


		public void OnFixtureFillingsChanged()
		{
			GalaxyChartFillingsEditorModel.FixtureFillingsChangedDelegate fixtureFillingsChanged = this.FixtureFillingsChanged;
			if (fixtureFillingsChanged == null)
			{
				return;
			}
			fixtureFillingsChanged();
		}


		public RandomFixtureFillingItem MakeNewRandomFixtureFillingItem()
		{
			List<FixtureFillingName> fixtureFillingNames = FixtureFillingNameConverter.NameProvider.GetFixtureFillingNames();
			if (fixtureFillingNames.Count == 0)
			{
				throw new InvalidOperationException("No Available Fixture Names");
			}
			return new RandomFixtureFillingItem(fixtureFillingNames[0], 1f);
		}


		private IEnumerable<GalaxyChartFillingsSource> fillingSources;


		private GalaxyChartFillingsSource selectedFillingsSource;


		private object selectedFillingsComponent;



		public delegate void FillingsSourcesChangedDelegate();



		public delegate void SelectedFillingsSourceChangedDelegate();



		public delegate void SelectedFillingsComponentChangedDelegate();



		public delegate void FixtureFillingsChangedDelegate();
	}
}
