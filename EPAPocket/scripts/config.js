Config = {};
Config.CustomerId = 1;
Config.AirlineId = 10;
//Config.User = {};
//Config.CurrentLocation = {};
// Config.serviceRoot = 'http://localhost:53121/';
//Config.webRoot = 'http://localhost:7386/';
//Config.reportRoot = 'http://localhost:9440/';
//Config.reportViewer = Config.reportRoot + 'viewer.aspx';
//Config.serviceUrl = Config.serviceRoot + 'api/';
//Config.serviceUrlOData = 'http://localhost:53121/OData/';
//Config.fileHandlerUrl = Config.webRoot + 'filehandler.ashx';
//Config.clientsFilesUrl = Config.webRoot + 'upload/images/clientsfiles/';

Config.Text_NoRowSelected = 'No Row(s) Selected';
Config.Text_NoFlightSelected = 'No Flight(s) Selected';
Config.Text_NoSarfaslSelected = 'هیچ سرفصلی انتخاب نشده است';
Config.Text_DeleteConfirm = 'The selected row will be deleted. Are you sure?';
Config.Text_SimpleConfirm = 'Are you sure?';
Config.Text_CanNotDelete = 'The selected cannot be deleted';
Config.Text_CanNotEdit = 'این ردیف قابل ویرایش نمی باشد';
Config.Text_FillRequired = 'Please fill in all required fields.';
Config.Text_SavedOk = 'The changes have been successfully saved.';
Config.Text_SameItemExist = 'Same item exists.';
Config.Text_GanttErrors = 'Gaps & Overlaps';
Config.Text_InvalidDates = 'Invalid Dates';
Config.Text_OffBlock = 'Off Block Value is invalid';
Config.Text_TakeOff = 'Take Off Value is invalid';
Config.Text_Landing = 'Landing Value is invalid';
Config.Text_OnBlock = 'On Block Value is invalid';
Config.LocalData = {};


/////////////////////////////////
Config.Types = [
    { type: 'airport', table: 'ViewAirport' },
   // { type: 'aidnc', table: 'ViewAid' },

];
Config.Fields = [
    //ViewAid
    { table: "ViewAirport", key: "CityId", value: "CityId" },
    { table: "ViewAirport", key: "Name", value: "Name" },
    { table: "ViewAirport", key: "IATA", value: "IATA" },
    { table: "ViewAirport", key: "ICAO", value: "ICAO" },


];

Config.MenuItems = [
    { moduleId: 2, key: 'library_book', title: 'Books', url: '/library/83/-1/-1', icon: '../../content/images/booksg.png' },
    { moduleId: 2, key: 'library_video', title: 'Videos', url: '/library/85/-1/-1', icon: '../../content/images/Videos2.png' },
    { moduleId: 2, key: 'library_paper', title: 'Papers', url: '/library/84/-1/-1', icon: '../../content/images/Papers2.png' },
    { moduleId: 2, key: 'library_document', title: 'Documents', url: '/document', icon: '../../content/images/docs2.png' },
    { moduleId: 2, key: 'library_people', title: 'People', url: '/person/book', icon: '../../content/images/study2.png' },

    { moduleId: 2, key: 'library_notification', title: 'Notifications', url: '/notification', icon: '../../content/images/notification2.png' },
    { moduleId: 2, key: 'library_publisher', title: 'Publishers', url: '/publisher', icon: '../../content/images/publisher2.png' },
    { moduleId: 2, key: 'library_author', title: 'Authors', url: '/author', icon: '../../content/images/quill2.png' },
    { moduleId: 2, key: 'library_journal', title: 'Journals', url: '/journal', icon: '../../content/images/newspaper2.png' },
    { moduleId: 2, key: 'library_conference', title: 'Conferences', url: '/journal', icon: '../../content/images/teamwork2.png' },



    { moduleId: 1, key: 'profile_person', title: 'Employees', url: '/person', icon: '../../content/images/group2.png' },
    { moduleId: 1, key: 'profile_course_person', title: 'Courses', url: '/course/person', icon: '../../content/images/course2.png' },
    { moduleId: 1, key: 'profile_person_certificate', title: 'Certificates', url: '/person/certificate', icon: '../../content/images/certificates2.png' },
    { moduleId: 1, key: 'profile_person_course', title: 'Employees Courses', url: '/person/course', icon: '../../content/images/setting2.png' },
    { moduleId: 1, key: 'profile_course', title: 'Archive', url: '/course', icon: '../../content/images/cabinets2.png' },
   
      { moduleId: 1, key: 'profile_course_type', title: 'Course Type', url: '/course/type', icon: '../../content/images/types2.png' },
   
    { moduleId: 1, key: 'profile_location', title: 'Departments', url: '/location', icon: '../../content/images/office2.png' },
    
    { moduleId: 1, key: 'profile_aircrafttype', title: 'Aircraft Types', url: '/aircrafttype', icon: '../../content/images/actype2.png' },
    
    { moduleId: 1, key: 'profile_educationfield', title: 'Education Fields', url: '/educationfield', icon: '../../content/images/fields2.png' },
    
    { moduleId: 1, key: 'profile_post', title: 'Posts', url: '/post', icon: '../../content/images/diagram2.png' },
  
      { moduleId: 1, key: 'profile_group', title: 'Groups', url: '/group', icon: '../../content/images/circle2.png' },
];

///////////////////////////////
Exceptions = {};
Exceptions.getMessage = function (error) {
    return { message:error.status+' '+ error.statusText+' '+error.data };
};
/////////////////////////////
Colors = {};
Colors.Palette = [
    
    '#ff275d',
    '#00b0f0',
    '#2cb77b',
    '#ffff00',
    '#ab85c3',
    '#a51a4d',
    '#7583ae',
    '#00FF00',
    '#ff9900',
    '#ff0000',
    '#5cffef',
    '#006395',
    '#ff0095',
    '#b4ff00',
    '#a11e9e',
    '#a11e38',
    '#a15c38',
    '#5a5c57',
    '#005f2a',
    '#00b2b1',
    '#6676ab',
    '#6676f6',
    '#661cf6',
    '#ff6100',
    '#3d3e34',
    '#7d9387',
    '#f6b2b1',
    '#f6b25a',
    '#f6765a',
    '#9f765a',
    '#9f76ab',
    
];
Colors.getRandom = function () {
    var color = '#' + (Math.random() * 0xFFFFFF << 0).toString(16);
    return color;
};

Colors.getColor = function (index) {
    if (index <= Colors.Palette.length - 1)
        return Colors.Palette[index];
    return Colors.getRandom ();
};
Colors.getColorReverse = function (index) {
    if (index <= Colors.Palette.length - 1)
        return Colors.Palette[Colors.Palette.length - 1-index];
    return Colors.getRandom();
};

/////////////////////////////////////




