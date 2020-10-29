<template>
  <div>
    <el-row id="search">
      创建时间：
      <el-date-picker v-model="input" type="date" placeholder="选择日期">
      </el-date-picker>
    </el-row>
    <el-table :data="tableData" style="width: 100%">
      <el-table-column
        fixed
        prop="date"
        label="创建时间"
        width="150"
        align="center"
      >
      </el-table-column>
      <el-table-column prop="name" label="任务详情" align="center">
      </el-table-column>
      <el-table-column fixed="right" label="操作" width="150" align="center">
        <template slot-scope="scope">
          <el-button @click="handleClick(scope.row)" type="success" size="small"
            >完成</el-button
          >
          <el-button type="danger" size="small">取消</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-row id="page">
      <el-pagination background layout="prev, pager, next" :total="1000">
      </el-pagination>
    </el-row>
  </div>
</template>
<script>
export default {
  data: function () {
    return {
      input: "",
      tableData: [
        {
          date: "2016-05-02",
          name: "王小虎",
          province: "上海",
          city: "普陀区",
          address: "上海市普陀区金沙江路 1518 弄",
          zip: 200333,
        },
        {
          date: "2016-05-04",
          name: "王小虎",
          province: "上海",
          city: "普陀区",
          address: "上海市普陀区金沙江路 1517 弄",
          zip: 200333,
        },
        {
          date: "2016-05-01",
          name: "王小虎",
          province: "上海",
          city: "普陀区",
          address: "上海市普陀区金沙江路 1519 弄",
          zip: 200333,
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          province: "上海",
          city: "普陀区",
          address: "上海市普陀区金沙江路 1516 弄",
          zip: 200333,
        },
        {
          date: "2016-05-03",
          name: "王小虎",
          province: "上海",
          city: "普陀区",
          address: "上海市普陀区金沙江路 1516 弄",
          zip: 200333,
        },
      ],
    };
  },
  methods: {
    handleClick(row) {
      console.log(row);
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
