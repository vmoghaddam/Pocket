app.controller('testController', ['$scope', '$location', '$routeParams', '$rootScope',  'authService', function ($scope, $location, $routeParams, $rootScope,  authService) {
    $scope.prms = $routeParams.prms;

    $scope.txt_control_value = 'لورم ایپسوم';
    $scope.txt_control = {
        hoverStateEnabled: false,
        width: 200,
        rtlEnabled:true,
        bindingOptions: {
           value:'txt_control_value',
        }
    };
    $scope.sb_control = {
        placeholder: 'انتخاب کنید',
        noDataText: 'موردی پیدا نشد',
        showClearButton: true,
        searchEnabled: true,
        dataSource: ['سبز', 'زرد', 'قرمز'],
        rtlEnabled: true,
        width: 400,
        bindingOptions: {
             
        }
    };


    $scope.dg_columns = [

        {
            cellTemplate: function (container, options) {
                $("<div style='text-align:center'/>")
                    .html(options.rowIndex + 1)
                    .appendTo(container);
            }, caption: '#', width: 60, allowResizing: false, fixed: true, fixedPosition: 'right',
            cssClass: 'rowHeader'
        },

        { dataField: "Title", caption: "عنوان", allowResizing: true, alignment: "right", dataType: 'string', allowEditing: false, width: 300, fixed: true, fixedPosition: 'right'},
        { dataField: "Connector", caption: "رابط", allowResizing: true, alignment: "right", dataType: 'string', allowEditing: false, width: 150 },
        { dataField: "Phone", caption: "تلفن", allowResizing: true, alignment: "center", dataType: 'string', allowEditing: false, width: 150 },

        { dataField: "Province", caption: "استان", allowResizing: true, alignment: "right", dataType: 'string', allowEditing: false, width: 150 },
        { dataField: "City", caption: "شهر", allowResizing: true, alignment: "right", dataType: 'string', allowEditing: false, width: 150 },
        { dataField: "Address", caption: "آدرس", allowResizing: true, alignment: "right", dataType: 'string', allowEditing: false, width: 300 },
        { dataField: "PostalCode", caption: "کد پستی", allowResizing: true, alignment: "center", dataType: 'string', allowEditing: false, width: 150 },

        { dataField: "Remark", caption: "توضیحات", allowResizing: true, alignment: "right", dataType: 'string', allowEditing: false, width:500 },


    ];
    $scope.dg_selected = null;

    $scope.dg_instance = null;
    $scope.dg_ds =  [{ Title: 'بهزیستی',ID:1 }, { Title: 'تامین اجتماعی',ID:2 }];
    $scope.dg = {
        showRowLines: true,
        showColumnLines: true,
        sorting: { mode: 'none' },
        filterRow: $rootScope.dg_filterRow,
         
        noDataText: '',
        rtlEnabled: true,
        allowColumnReordering: true,
        allowColumnResizing: true,
        scrolling: { mode: 'infinite' },
        paging: { pageSize: 100 },
        showBorders: true,
        selection: { mode: 'single' },
        loadPanel: {
            enabled: false,
            text: 'لطفا صبر کنید'

        },
        height: 400,
        width:'100%',
        columns: $scope.dg_columns,
        onContentReady: function (e) {
            if (!$scope.dg_instance)
                $scope.dg_instance = e.component;

        },
        onSelectionChanged: function (e) {
            var data = e.selectedRowsData[0];

            if (!data) {
                $scope.dg_selected = null;
            }
            else
                $scope.dg_selected = data;


        },
        bindingOptions: {
            dataSource: 'dg_ds'
        }
    };

    /////////////////////////
    if (!authService.isAuthorized()) {

        authService.redirectToLogin();
    }
    else {
        $rootScope.page_title = ':'+' تست';
        $('.test').fadeIn();
    }
    ////////////////////////////////
    $scope.$on('$viewContentLoaded', function () {
    });


    ////End Of Controller
}]);
