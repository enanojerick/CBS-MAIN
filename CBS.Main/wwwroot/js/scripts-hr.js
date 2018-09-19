$(document).ready(function () {

    /** datatable INIT */
    $('.sortingtables').DataTable();

    /** Tab */
    $("div.custom-tab-menu>div.list-group>a").click(function (e) {
        e.preventDefault();
        $(this).siblings('a.active').removeClass("active");
        $(this).addClass("active");
        var index = $(this).index();
        $("div.custom-tab>div.custom-tab-content").removeClass("active");
        $("div.custom-tab>div.custom-tab-content").eq(index).addClass("active");
    });


    /* Image Upload */
    var img = {
        jcropApi: '',
        imageCropWidth: 0,
        imageCropHeight: 0,
        cropPointX: 0,
        cropPointY: 0
    };

    var readFile = function (input) {

        if (input.files && input.files[0]) {

            var reader = new FileReader();
            if (img.jcropApi !== '') {
                img.jcropApi.destroy();

            }

            reader.onload = function (e) {

                jQuery("#cropModal").modal({
                    backdrop: 'static'
                });

                $('#imgEmpPhoto').attr('src', "");

                var imgs = $('#imgEmpPhoto').attr('src', e.target.result);
                var width = imgs[0].height;
                var height = imgs[0].width;

                initCrop();

            }

            reader.readAsDataURL(input.files[0]);
        }
    }

    var initCrop = function () {
        var box_width = $('#image_prev_container').width();

        $('#imgEmpPhoto').Jcrop({
            setSelect: [0, 0, 320],
            minSize: [52, 140],
            aspectRatio: 4 / 1,
            keySupport: false,
            boxWidth: box_width,
            onChange: setCoordsAndImgSize,
            //aspectRatio: 1,
            onSelect: setCoordsAndImgSize

        }, function () { img.jcropApi = this });
    }

    var setCoordsAndImgSize = function (e) {
        console.log(img);
        img.imageCropWidth = e.w;
        img.imageCropHeight = e.h;
        img.cropPointX = e.x;
        img.cropPointY = e.y;
    }

    var cropImage = function () {
        console.log(img);
        if (img.imageCropWidth === 0 && img.imageCropHeight === 0) {

            alert("Please select crop area.");

            return;

        }

        var imgs = $("#imgEmpPhoto").attr("src");

        showCroppedImage();

    }

    var showCroppedImage = function () {

        var x1 = img.cropPointX;

        var y1 = img.cropPointY;

        var width = img.imageCropWidth;

        var height = img.imageCropHeight;

        var canvas = $("#canvas")[0];

        var context = canvas.getContext('2d');

        var imgs = new Image();

        imgs.onload = function () {

            canvas.height = height;

            canvas.width = width;

            context.drawImage(imgs, x1, y1, width, height, 0, 0, width, height);
            console.log(canvas);
            console.log(canvas.toDataURL());
            $('#CropImage').val(canvas.toDataURL());

            $("#canvas").hide();
        };

        imgs.src = $('#imgEmpPhoto').attr("src");

    }

// #region Upload Excel

    ////Short Course
    //$('#btnShortCUpload').on('click', function () {
    //    var fileExtension = ['xls', 'xlsx'];
    //    var filename = $('#ShortCUpload').val();
    //    if (filename.length == 0) {
    //        alert("Please select a file.");
    //        return false;
    //    }
    //    else {
    //        var extension = filename.replace(/^.*\./, '');
    //        if ($.inArray(extension, fileExtension) == -1) {
    //            alert("Please select only excel files.");
    //            return false;
    //        }
    //    }
    //    var fdata = new FormData();
    //    var fileUpload = $("#certUpload").get(0);
    //    var files = fileUpload.files;
    //    fdata.append(files[0].name, files[0]);
    //    $.ajax({
    //        type: "POST",
    //        url: "/ShortCourse/ExcelUploadShortCourses",
    //        data: fdata,
    //        contentType: false,
    //        processData: false,
    //        success: function (response) {
    //        },
    //        error: function (e) {
    //        }
    //    });
    //})

    ////Certification
    //$('#btnCertUpload').on('click', function () {
    //    var fileExtension = ['xls', 'xlsx'];
    //    var filename = $('#certUpload').val();
    //    if (filename.length == 0) {
    //        alert("Please select a file.");
    //        return false;
    //    }
    //    else {
    //        var extension = filename.replace(/^.*\./, '');
    //        if ($.inArray(extension, fileExtension) == -1) {
    //            alert("Please select only excel files.");
    //            return false;
    //        }
    //    }
    //    var fdata = new FormData();
    //    var fileUpload = $("#certUpload").get(0);
    //    var files = fileUpload.files;
    //    fdata.append(files[0].name, files[0]);
    //    $.ajax({
    //        type: "POST",
    //        url: "/Certification/ExcelUploadCertifications",
    //        data: fdata,
    //        contentType: false,
    //        processData: false,
    //        success: function (response) {
    //        },
    //        error: function (e) {
    //        }
    //    });
    //});

    //// Education
    //$('#btnEduUpload').on('click', function () {
    //    var fileExtension = ['xls', 'xlsx'];
    //    var filename = $('#eduUpload').val();
    //    if (filename.length == 0) {
    //        alert("Please select a file.");
    //        return false;
    //    }
    //    else {
    //        var extension = filename.replace(/^.*\./, '');
    //        if ($.inArray(extension, fileExtension) == -1) {
    //            alert("Please select only excel files.");
    //            return false;
    //        }
    //    }
    //    var fdata = new FormData();
    //    var fileUpload = $("#eduUpload").get(0);
    //    var files = fileUpload.files;
    //    fdata.append(files[0].name, files[0]);
    //    $.ajax({
    //        type: "POST",
    //        url: "/Education/ExcelUploadEducation",
    //        data: fdata,
    //        contentType: false,
    //        processData: false,
    //        success: function (response) {
    //        },
    //        error: function (e) {
    //        }
    //    });
    //});

    ////On Boarding
    //$('#btnUpload').on('click', function () {
    //    var fileExtension = ['xls', 'xlsx'];
    //    var filename = $('#fUpload').val();
    //    if (filename.length == 0) {
    //        alert("Please select a file.");
    //        return false;
    //    }
    //    else {
    //        var extension = filename.replace(/^.*\./, '');
    //        if ($.inArray(extension, fileExtension) == -1) {
    //            alert("Please select only excel files.");
    //            return false;
    //        }
    //    }
    //    var fdata = new FormData();
    //    var fileUpload = $("#fUpload").get(0);
    //    var files = fileUpload.files;
    //    fdata.append(files[0].name, files[0]);
    //    $.ajax({
    //        type: "POST",
    //        url: "/OnBoarding/ExcelUploadStaffmembers",
    //        data: fdata,
    //        contentType: false,
    //        processData: false,
    //        success: function (response) {
    //            //if (response.length == 0)
    //            //    alert('Some error occured while uploading');
    //            //else {
    //            //    $('#dvData').html(response);
    //            //}
    //        },
    //        error: function (e) {
    //            //$('#dvData').html(e.responseText);
    //        }
    //    });
    //});
// #endregion


});

var getChildren = function (data) {
    var childData = [];
    jQuery.each(data, function (k, v) {
        var branch = {};

        branch.name = v.organizationName;
        branch.title = v.industry;
        branch.orgId = v.organizationID;
        branch.orgType = v.organizationTypeID;

        if (v.organizationBranch.length > 0) {
            branch.children = getChildren(v.organizationBranch);
            branch.className = "parentBranch org-" + branch.orgId;
        } else {
            if (v.organizationTypeID === 1) {
                branch.className = "parentBranch org-" + branch.orgId;
            } else {
                branch.className = "index-" + k + " org-" + branch.orgId;
            }
           
        }

        childData.push(branch);
    });

    return childData;

}
