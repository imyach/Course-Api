using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CourseDesktopClient.UI.Elements
{
    public partial class RatingStarsControl : UserControl
    {
        private bool _isRatingSelected = false;
        private int _selectedRating = 0;
        private int _hoverRating = 0;
        private Button[] _starButtons;

        public static readonly DependencyProperty RatingCommandProperty =
            DependencyProperty.Register(
                nameof(RatingCommand),
                typeof(ICommand),
                typeof(RatingStarsControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register(
                nameof(Rating),
                typeof(int),
                typeof(RatingStarsControl),
                new PropertyMetadata(0, OnRatingChanged));

        public static readonly DependencyProperty ResetRatingProperty =
    DependencyProperty.Register(
        nameof(ResetRating),
        typeof(bool),
        typeof(RatingStarsControl),
        new PropertyMetadata(false, OnResetRatingChanged));

        public bool ResetRating
        {
            get => (bool)GetValue(ResetRatingProperty);
            set => SetValue(ResetRatingProperty, value);
        }
        private bool _lastResetValue = false;
        private static void OnResetRatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RatingStarsControl control)
            {
                bool newValue = (bool)e.NewValue;

                // Если значение изменилось на true
                if (newValue && !control._lastResetValue)
                {
                    control.ResetStars();
                }

                control._lastResetValue = newValue;
            }
        }

        public void ResetStars()
        {
            _isRatingSelected = false;
            _selectedRating = 0;
            UpdateStars(0);
            UpdateRatingText();
        }


        public ICommand RatingCommand
        {
            get => (ICommand)GetValue(RatingCommandProperty);
            set => SetValue(RatingCommandProperty, value);
        }

        public int Rating
        {
            get => (int)GetValue(RatingProperty);
            set => SetValue(RatingProperty, value);
        }

        public RatingStarsControl()
        {
            InitializeComponent();
            InitializeStars();
        }

        private void InitializeStars()
        {
            _starButtons = new[] { btnStar1, btnStar2, btnStar3, btnStar4, btnStar5 };

            // Устанавливаем CommandParameter для каждой кнопки
            for (int i = 0; i < _starButtons.Length; i++)
            {
                _starButtons[i].CommandParameter = (i + 1).ToString();
            }
        }

        private void StarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string parameter)
            {
                if (int.TryParse(parameter, out int rating))
                {
                    _selectedRating = rating;
                    _isRatingSelected = true;
                    Rating = _selectedRating;

                    UpdateStars(_selectedRating);
                    UpdateRatingText();

                    // Выполняем команду
                    RatingCommand?.Execute(rating);
                }
            }
        }

        private void StarButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string parameter)
            {
                if (int.TryParse(parameter, out int hoverIndex))
                {
                    _hoverRating = hoverIndex;

                    if (!_isRatingSelected)
                    {
                        // Если оценка еще не выбрана, подсвечиваем все до наведенной
                        UpdateStars(_hoverRating, isPreview: false);
                    }
                    else
                    {
                        // Если оценка выбрана, показываем временную подсветку для изменения
                        UpdateStars(_hoverRating, isPreview: true);
                    }
                }
            }
        }

        private void StarButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_isRatingSelected)
            {
                // Возвращаем сохраненную оценку
                UpdateStars(_selectedRating);
            }
            else
            {
                // Сбрасываем все звезды
                UpdateStars(0);
            }
            _hoverRating = 0;
        }

        private void UpdateStars(int count, bool isPreview = false)
        {
            var selectedColor = isPreview ? Brushes.Goldenrod : Brushes.Gold;

            for (int i = 0; i < _starButtons.Length; i++)
            {
                if (i < count)
                {
                    // Подсвечиваем звезду
                    _starButtons[i].Tag = selectedColor;
                    _starButtons[i].Foreground = selectedColor;
                }
                else
                {
                    // Возвращаем серый цвет
                    _starButtons[i].Tag = Brushes.LightGray;
                    _starButtons[i].Foreground = Brushes.LightGray;
                }
            }
        }

        private void UpdateRatingText()
        {
            txtRating.Text = _selectedRating > 0 ? $"{_selectedRating}.0" : "0.0";
        }

        private static void OnRatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RatingStarsControl control)
            {
                int newRating = (int)e.NewValue;

                if (newRating >= 0 && newRating <= 5)
                {
                    control._selectedRating = newRating;
                    control._isRatingSelected = newRating > 0;
                    control.UpdateStars(newRating);
                    control.UpdateRatingText();
                }
            }
        }

        // Метод для установки рейтинга извне
        public void SetRating(int rating)
        {
            if (rating >= 0 && rating <= 5)
            {
                _selectedRating = rating;
                _isRatingSelected = rating > 0;
                Rating = rating;
                UpdateStars(rating);
                UpdateRatingText();
            }
        }

        // Свойство только для чтения, чтобы получить текущий рейтинг
        public int CurrentRating => _selectedRating;
    }
}