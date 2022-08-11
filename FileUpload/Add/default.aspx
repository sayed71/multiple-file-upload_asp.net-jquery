<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Master/Panel.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="FileUpload.Add._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../../Assets/script/library/jquery-1.7.2.min.js"></script>
    <script type="text/javascript">
        var jQuery_1_7_2 = $.noConflict(true);
    </script>
    <script src="default.js"></script>
    
    <script type="text/javascript">
        //var jQuery_1_7_2 = $.noConflict(true);
        var jQuery_1_4_4 = $.noConflict(true);
    </script>
    
    <div class="content-wrapper" style="padding: 0; margin: 0;">
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="box-header with-border">
                                <div id="msg"></div>
                                <h3 class="box-title">New</h3>
                            </div>
                            <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label for="Name">Ref. No</label><span style="color:red;"> *</span>
                                        <input type="text" class="form-control required" readonly placeholder="Reference No" id="txtNo" name="txtNo" />
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label for="Name">Date</label><span style="color:red;"> *</span>
                                        <input type="text" class="form-control show_datepicker required" placeholder="dd/mm/yyyy" id="txtDate" name="txtDate" />
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-8">
                                    <input id="hdn_master_id" type="hidden" />
                                    <input id="hdn_count_element" type="hidden" value="1" />
                                    <input id="hdn_remove_all_id" type="hidden" />
                                    <div class="form-group">
                                        <div class="table-responsive">
                                            <table class="table table-hover table-bordered" id="tbl_details" style="min-width: 600px;">
                                                <thead>
                                                    <tr>
                                                        <th style="width: 65%">File Name</th>
                                                        <th style="width: 25%">Browse</th>
                                                        <th colspan="2" style="width: 5%; text-align: center; margin: 0 auto;" align="center"><span class="glyphicon glyphicon-cog"></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <%-- empty_details_data() Function work for comments Tag--%>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label for="Name">Regard</label>
                                        <textarea class="form-control required" rows="2" id="txtRegard" name="txtRegard"></textarea>
                                    </div>
                                </div>
                            </div>

                            <div class="box-footer">
                                <button id="btnsave" type="button" onclick="save()" class="btn btn-primary">Save</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>

    <asp:HiddenField ID="hfCustomerId" runat="server" />
    
</asp:Content>
