   M S B A M L   `   `   `  ����D  @NLTestApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:NLTestApp.ViewModelsNLTestApp.ViewModels  5          NLTestApp.Views.MainView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      9
viewModels"clr-namespace:NLTestApp.ViewModels     5        � DataContext  5      (  "NLTestApp.ViewModels.MainViewModel  5   
   .��5      � .+�5	   
   ��  �� Orientation$ 
Horizontal=�6   .+�5
      �� .+�5      &   System.Windows.Controls.DataGrid     AutoGenerateColumns .     CanUserDeleteRows .  $
��Auto��6r      CanUserAddRows .     CanUserReorderColumns .  ��6   �� *
Martians+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ФИО��6-   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6:   �� *
FullName+  5      $ Дата рождения��6-    6D   �� *	BirthDate+  5      $ Email��6-    6<   �� *Email+  5      $ Телефон��6-    6>   �� *Phone+4  .System.Windows.Controls.DataGridTemplateColumn  5      	   CellTemplate	 5      �� 5   "   .R�5   &   �� $��-��6J   $
��Bold"�6V   ��6-   �� ��6i   �� 
 �� RelativeSource
 �� *FindAncestor+ �� AncestorType$ 
StackPanelL� �� Path$&  DataContext.RemoveMartianCommand�5   #   5      5      �� 5      $ 
Horizontal=�6   .+�5      �� $��80��6F   $��
Назад��6Q   $��5q�6a   ��6   �� *DecrementPageCommand+6a   �� 5      $��Страница��6    $��5q�60   �� 5      $
��Bold"�6:   $��5q�6M   ��6    �� *
UIPageNo+6M   �� 5      $
��из��6    $��5q�6*   �� 5      $
��Bold"�69   $��5q�6L   ��6    �� *	MaxPage+6L   �� 5       $��80��6F   $��Вперед��6Q   $��5q�6b   ��6   �� *IncrementPageCommand+6b   5!      5"      �� 5#      $
��Auto��6   .+�5$      �� $)��#Загрузить TSV/CSV файл��6?   $	��5 5q�6`   ��6   �� *ImportDataCommand+6`   �� 5%      $,��&Обновить базу данных��6;   $	��5 5q�6Z   ��6   �� *UpdateCommand+6Z   �� 5&      $!��Экспорт данных��6?   $	��5 5q�6X   ��6   �� *ExportDataCommand+6X   �� 5'      $��Настройки��6G   $	��5 5q�6[   ��6   �� *OpenSettingsDialogCommand+6[   5(      5)      5*      5+      