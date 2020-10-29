<template>
  <div>
    <el-row id="search">
      <el-col :span="10">
        <span>创建时间:</span>
        <el-date-picker
          v-model="createTime"
          type="daterange"
          range-separator="至"
          start-placeholder="开始日期"
          end-placeholder="结束日期"
          @change="bind"
        >
        </el-date-picker>
      </el-col>
      <el-col :span="10">
        <span>任务状态:</span>
        <el-select v-model="state" placeholder="请选择" @change="this.bind">
          <el-option
            v-for="item in options"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          >
          </el-option>
        </el-select>
      </el-col>
      <el-col :span="4" style="text-align: center">
        <el-button size="medium" @click="create">新建</el-button>
      </el-col>
    </el-row>
    <el-table :data="tableData" style="width: 100%">
      <el-table-column
        fixed
        prop="createTime"
        label="创建时间"
        width="150"
        align="center"
      >
      </el-table-column>
      <el-table-column prop="text" label="任务详情" align="center">
      </el-table-column>
      <el-table-column fixed="right" label="操作" width="150" align="center">
        <template slot-scope="scope" v-if="scope.row.state === 1">
          <el-button @click="update(scope.row, 2)" type="success" size="small"
            >完成</el-button
          >
          <el-button type="danger" size="small" @click="update(scope.row, 3)"
            >取消</el-button
          >
        </template>
      </el-table-column>
    </el-table>
    <el-row id="page">
      <el-pagination
        background
        layout="prev, pager, next"
        :total="total"
        :page-size="5"
        :current-page="page"
        @current-change="pageChange"
      >
      </el-pagination>
    </el-row>
    <el-dialog title="创建任务" :visible.sync="dialogFormVisible">
      <el-form :model="form">
        <el-form-item label="任务内容" :label-width="formLabelWidth">
          <el-input
            type="textarea"
            :rows="2"
            placeholder="请输入内容"
            v-model="textarea"
          >
          </el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="cancel">取 消</el-button>
        <el-button type="primary" @click="confirm">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>
<script>
import axios from "axios";

export default {
  data: function () {
    return {
      page: 1,
      total: 0,
      options: [
        {
          label: "待完成",
          value: 1,
        },
        {
          label: "已完成",
          value: 2,
        },
        {
          label: "已取消",
          value: 3,
        },
      ],
      state: 1,
      input: "",
      textarea: "",
      dialogFormVisible: false,
      createTime: "",
      formLabelWidth: "120px",
      form: {
        name: "",
        region: "",
        date1: "",
        date2: "",
        delivery: false,
        type: [],
        resource: "",
        desc: "",
      },
      tableData: [],
    };
  },
  mounted() {
    this.bind();
  },
  methods: {
    pageChange(page) {
      this.page = page;
      this.bind();
    },
    update(row, state) {
      axios
        .put("http://localhost:5000/api/plan/" + row.id, {
          state: state,
        })
        .then((r) => {
          this.$message({
            message: "操作成功!",
            type: "success",
          });
          this.bind();
        })
        .catch((e) => {
          this.$message.error("操作失败!");
        });
    },
    bind() {
      axios
        .get("http://localhost:5000/api/plan", {
          params: {
            startTime: this.createTime[0],
            endTime: this.createTime[1],
            page: this.page,
            size: 5,
            state: this.state,
          },
        })
        .then((r) => {
          this.tableData = r.data.data;
          this.total = r.data.totalCount;
        })
        .catch((e) => {
          this.$message.error("获取数据失败!");
        });
    },
    handleClick(row) {
      console.log(row);
    },
    create() {
      this.dialogFormVisible = true;
    },
    confirm() {
      axios
        .post("http://localhost:5000/api/plan", {
          text: this.textarea,
        })
        .then((r) => {
          this.dialogFormVisible = false;
          this.textarea = "";
          this.$message({
            message: "创建成功!",
            type: "success",
          });
          this.bind();
        })
        .catch((e) => {
          this.$message.error("创建失败!");
        });
    },
    cancel() {
      this.textarea = "";
      this.dialogFormVisible = false;
    },
  },
};
</script>
<style scoped>
#page {
  margin: 15px;
  text-align: center;
}
#search {
  margin: 15px;
}
</style>
