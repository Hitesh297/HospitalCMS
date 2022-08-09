window.onload = function () {
    $('#speciality-list').change(function () {
        var specialityId = $(this).val();
        var employees = $('#doctor-list');
        employees.empty();
        // clear existing options
        $.ajax({
            url: 'https://localhost:44305/api/DoctorData/ListDoctorsBySpeciality/' + specialityId, // do not hard code url's
            type: "GET",
            dataType: "json",
            success: function (data) {
                if (data) {
                    console.log("Successfull!", data);
                    employees.append($('<option></option>').val("X").html("Please Select"));
                    $.each(data, function (index, item) {
                        employees.append($('<option></option>').val(item.DoctorId).html(item.Name));
                    });
                } else {
                    // oops
                }
            },
            error: function (err) {
                console.log("Error occured!");
                alert(err);
            }
        });
    });

    $("#imgInput").change(function () {
        readURL(this);
    });
    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#profile-pic').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#eventPic").change(function () {
        readeventURL(this);
    });
    function readeventURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#event-banner').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#articlePic").change(function () {
        readarticleURL(this);
    });
    function readarticleURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#article-banner').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }


    $(function () {
        $(".accordion").accordion(
            { header: "h3", collapsible: true, active: false }
        );
    });

    var selectedGender = $("#GenderValue").val();
    $('#Gender').val(selectedGender);

    var selectedMaritalStatus = $("#MaritalStatusValue").val();
    $('#MaritalStatus').val(selectedMaritalStatus);
}

