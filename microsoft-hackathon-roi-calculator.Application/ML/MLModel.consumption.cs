﻿// This file was auto-generated by ML.NET Model Builder.
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Microsoft_hackathon_roi_calculator_Application
{
    public partial class MLModel
    {
        /// <summary>
        /// model input class for MLModel.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [LoadColumn(1)]
            [ColumnName(@"ProjectBudget")]
            public float ProjectBudget { get; set; }

            [LoadColumn(2)]
            [ColumnName(@"NumberOfEmployees")]
            public float NumberOfEmployees { get; set; }

            [LoadColumn(3)]
            [ColumnName(@"ProjectDurationMonths")]
            public float ProjectDurationMonths { get; set; }

            [LoadColumn(4)]
            [ColumnName(@"ROI")]
            public float ROI { get; set; }

        }

        #endregion

        /// <summary>
        /// model output class for MLModel.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            [ColumnName(@"ProjectBudget")]
            public float ProjectBudget { get; set; }

            [ColumnName(@"NumberOfEmployees")]
            public float NumberOfEmployees { get; set; }

            [ColumnName(@"ProjectDurationMonths")]
            public float ProjectDurationMonths { get; set; }

            [ColumnName(@"ROI")]
            public float ROI { get; set; }

            [ColumnName(@"Features")]
            public float[] Features { get; set; }

            [ColumnName(@"Score")]
            public float Score { get; set; }

        }

        #endregion

        private static string MLNetModelPath = Path.Combine(Directory.GetCurrentDirectory(), "MLModel.mlnet");

        public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);


        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var _);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }

        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(ModelInput input)
        {
            var predEngine = PredictEngine.Value;
            return predEngine.Predict(input);
        }
    }
}
